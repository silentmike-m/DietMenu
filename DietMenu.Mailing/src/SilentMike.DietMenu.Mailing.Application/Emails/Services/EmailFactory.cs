namespace SilentMike.DietMenu.Mailing.Application.Emails.Services;

using HtmlAgilityPack;
using SilentMike.DietMenu.Mailing.Application.Emails.Models;
using SilentMike.DietMenu.Mailing.Application.Extensions;
using SilentMike.DietMenu.Mailing.Application.Interfaces;

public sealed class EmailFactory
{
    private const string LOGO_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.logo64x64.png";

    private readonly IXmlService xmlService;

    public EmailFactory(IXmlService xmlService)
        => this.xmlService = xmlService;

    public async Task<Email> CreateEmailAsync(string receiver, string subject, string text, CancellationToken cancellationToken = default)
    {
        var emailLinkedResources = await CreateLogoContentAsync(LOGO_RESOURCE_NAME, cancellationToken);

        var email = new Email(text, emailLinkedResources, receiver, subject, text);

        return email;
    }

    public async Task<Email> CreateEmailAsync(string receiver, string requestXml, string subject, string xsltHtmlResourceName, string xsltTextResourceName, CancellationToken cancellationToken = default)
    {
        var htmlMessage = await this.GetHmlMessageAsync(xsltHtmlResourceName, requestXml, cancellationToken);

        var textMessage = await this.GetTextMessageAsync(xsltTextResourceName, requestXml, cancellationToken);

        var emailLinkedResources = await CreateLogoContentAsync(LOGO_RESOURCE_NAME, cancellationToken);

        var email = new Email(htmlMessage, emailLinkedResources, receiver, subject, textMessage);

        return email;
    }

    private async Task<string> GetHmlMessageAsync(string resourceName, string requestXml, CancellationToken cancellationToken)
    {
        var htmlXsltString = await this.xmlService.GetXsltStringAsync(resourceName, cancellationToken);
        var html = this.xmlService.TransformToHtml(requestXml, htmlXsltString);
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        return htmlDoc.DocumentNode.InnerHtml;
    }

    private async Task<string> GetTextMessageAsync(string resourceName, string requestXml, CancellationToken cancellationToken)
    {
        var textXsltString = await this.xmlService.GetXsltStringAsync(resourceName, cancellationToken);
        var text = this.xmlService.TransformToText(requestXml, textXsltString);

        return text;
    }

    private static async Task<List<EmailLinkedResource>> CreateLogoContentAsync(string resourceName, CancellationToken cancellationToken = default)
    {
        var logoData = await resourceName.GetResourceBytesAsync(cancellationToken);

        var emailLinkedResources = new List<EmailLinkedResource>
        {
            new("logoId", logoData, "logo64x64.png"),
        };

        return emailLinkedResources;
    }
}
