namespace SilentMike.DietMenu.Mailing.Application.Interfaces;

public interface IXmlService
{
    string GetXsltString(string resourceName);
    string TransformToHtml(string xmlString, string xsltString);
    string TransformToText(string xmlString, string xsltString);
}
