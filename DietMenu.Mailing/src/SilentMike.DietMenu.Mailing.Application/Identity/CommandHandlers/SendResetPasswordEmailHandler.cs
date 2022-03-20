namespace SilentMike.DietMenu.Mailing.Application.Identity.CommandHandlers;

using System.Reflection;
using System.Xml.Serialization;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Emails.Models;
using SilentMike.DietMenu.Mailing.Application.Exceptions;
using SilentMike.DietMenu.Mailing.Application.Extensions;
using SilentMike.DietMenu.Mailing.Application.Identity.Commands;
using SilentMike.DietMenu.Mailing.Application.Interfaces;

internal sealed class SendResetPasswordEmailHandler : IRequestHandler<SendResetPasswordEmail>
{
    private const string EMAIL_SUBJECT = "Reset Your DietMenu account password";
    private const string LOGO_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.logo64x64.png";
    private const string XSLT_HTML_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.Identity.ResetPasswordHtmlEmail.xslt";
    private const string XSLT_PLAIN_TEXT_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.Identity.ResetPasswordPlainTextEmail.xslt";


    private readonly ILogger<SendResetPasswordEmailHandler> logger;
    private readonly IMediator mediator;
    private readonly IXmlService xmlService;

    public SendResetPasswordEmailHandler(
        ILogger<SendResetPasswordEmailHandler> logger,
        IMediator mediator,
        IXmlService xmlService)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.xmlService = xmlService;
    }

    public async Task<Unit> Handle(SendResetPasswordEmail request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email)
        );

        this.logger.LogInformation("Try to prepare reset password email");

        var serializer = new XmlSerializer(typeof(SendResetPasswordEmail));
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
            Subject = EMAIL_SUBJECT,
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
