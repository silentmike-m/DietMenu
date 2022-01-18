﻿namespace SilentMike.DietMenu.Mailing.Application.Users.CommandHandlers;

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Application.Common;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Emails.ViewModels;
using SilentMike.DietMenu.Mailing.Application.Exceptions;
using SilentMike.DietMenu.Mailing.Application.Extensions;
using SilentMike.DietMenu.Mailing.Application.Interfaces;
using SilentMike.DietMenu.Mailing.Application.Users.Commands;

internal sealed class SendCreatedUserEmailHandler : IRequestHandler<SendCreatedUserEmail>
{
    private const string LOGO_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.logo64x64.png";
    private const string XSLT_HTML_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.Users.CreatedUserHtmlEmail.xslt";
    private const string XSLT_PLAIN_TEXT_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.Users.CreatedUserPlainTextEmail.xslt";


    private readonly ILogger<SendCreatedUserEmailHandler> logger;
    private readonly IMediator mediator;
    private readonly IXmlService xmlService;

    public SendCreatedUserEmailHandler(
        ILogger<SendCreatedUserEmailHandler> logger,
        IMediator mediator,
        IXmlService xmlService)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.xmlService = xmlService;
    }

    public async Task<Unit> Handle(SendCreatedUserEmail request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email),
            ("FamilyName", request.FamilyName),
            ("UserName", request.UserName)
        );

        this.logger.LogInformation("Try to prepare created user email");

        var serializer = new XmlSerializer(typeof(SendCreatedUserEmail));
        var requestXml = serializer.Serialize(request);

        var htmlXsltString = this.xmlService.GetXsltString(XSLT_HTML_RESOURCE_NAME);
        var html = this.xmlService.TransformToHtml(requestXml, htmlXsltString);
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var textXsltString = this.xmlService.GetXsltString(XSLT_PLAIN_TEXT_RESOURCE_NAME);
        var text = this.xmlService.TransformToText(requestXml, textXsltString);

        var logoData = await GetResourceBytes(LOGO_RESOURCE_NAME);

        var emailLinkedResources = new List<EmailLinkedResource>
        {
            new()
            {
                ContentId = "logoId",
                Data = logoData,
                FileName = "logo64x64.png",
            },
        };

        var email = new Email
        {
            HtmlMessage = htmlDoc.DocumentNode.InnerHtml,
            LinkedResources = emailLinkedResources,
            Receiver = request.Email,
            Subject = "Create Your DietMenu account",
            TextMessage = text,
        };

        var command = new SendEmail
        {
            Email = email,
        };

        await this.mediator.Send(command, cancellationToken);


        return await Task.FromResult(Unit.Value);
    }

    private static async Task<byte[]> GetResourceBytes(string resourceName)
    {
        await using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

        if (resourceStream is null)
        {
            throw new ResourceNotFoundException(resourceName);
        }

        await using var memoryStream = new MemoryStream();
        await resourceStream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}
