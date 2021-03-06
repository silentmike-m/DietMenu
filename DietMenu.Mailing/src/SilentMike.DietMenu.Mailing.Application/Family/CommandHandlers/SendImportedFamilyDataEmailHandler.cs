namespace SilentMike.DietMenu.Mailing.Application.Family.CommandHandlers;

using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Emails.Models;
using SilentMike.DietMenu.Mailing.Application.Exceptions;
using SilentMike.DietMenu.Mailing.Application.Extensions;
using SilentMike.DietMenu.Mailing.Application.Family.Commands;
using SilentMike.DietMenu.Mailing.Application.Identity.Queries;
using SilentMike.DietMenu.Mailing.Application.Interfaces;

internal sealed class SendImportedFamilyDataEmailHandler : IRequestHandler<SendImportedFamilyDataEmail>
{
    private const string EMAIL_SUBJECT = "Family data imported";
    private const string LOGO_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.logo64x64.png";
    private const string XSLT_HTML_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.Family.ImportedFamilyDataHtmlEmail.xslt";
    private const string XSLT_PLAIN_TEXT_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.Family.ImportedFamilyDataTextEmail.xslt";


    private readonly ILogger<SendImportedFamilyDataEmailHandler> logger;
    private readonly IMediator mediator;
    private readonly IXmlService xmlService;

    public SendImportedFamilyDataEmailHandler(
        ILogger<SendImportedFamilyDataEmailHandler> logger,
        IMediator mediator,
        IXmlService xmlService)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.xmlService = xmlService;
    }

    public async Task<Unit> Handle(SendImportedFamilyDataEmail request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId)
        );

        this.logger.LogInformation("Try to prepare family data imported email");

        var getFamilyUserEmail = new GetFamilyUserEmail
        {
            FamilyId = request.FamilyId,
        };

        var familyUserEmail = await this.mediator.Send(getFamilyUserEmail, cancellationToken);

        var serializer = new XmlSerializer(typeof(SendImportedFamilyDataEmail));
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
            Receiver = familyUserEmail,
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
