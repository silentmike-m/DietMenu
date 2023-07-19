namespace SilentMike.DietMenu.Mailing.Application.Interfaces;

public interface IXmlService
{
    Task<string> GetXsltStringAsync(string resourceName, CancellationToken cancellationToken = default);
    string TransformToHtml(string xmlString, string xsltString);
    string TransformToText(string xmlString, string xsltString);
}
