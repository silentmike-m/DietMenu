namespace SilentMike.DietMenu.Mailing.Application.Services;

using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using SilentMike.DietMenu.Mailing.Application.Exceptions;
using SilentMike.DietMenu.Mailing.Application.Interfaces;

internal sealed class XmlService : IXmlService
{
    public string GetXsltString(string resourceName)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

        if (stream is null)
        {
            throw new ResourceNotFoundException(resourceName);
        }

        var xslt = new StreamReader(stream).ReadToEnd();
        return xslt;
    }

    public string TransformToHtml(string xmlString, string xsltString)
    {
        using var xsltReader = XmlReader.Create(new StringReader(xsltString));
        var transform = new XslCompiledTransform();
        transform.Load(xsltReader);

        using var xmlReader = XmlReader.Create(new StringReader(xmlString));

        var stringBuilder = new StringBuilder();
        var settings = transform.OutputSettings?.Clone() ?? new XmlWriterSettings();
        settings.ConformanceLevel = ConformanceLevel.Document;

        using var xmlWriter = XmlWriter.Create(new StringWriter(stringBuilder), settings);
        transform.Transform(xmlReader, xmlWriter);

        return stringBuilder.ToString();
    }

    public string TransformToText(string xmlString, string xsltString)
    {
        using var xsltReader = XmlReader.Create(new StringReader(xsltString));
        var transform = new XslCompiledTransform();
        transform.Load(xsltReader);

        using var xmlReader = XmlReader.Create(new StringReader(xmlString));

        var stringBuilder = new StringBuilder();
        using var xmlWriter = XmlWriter.Create(new StringWriter(stringBuilder), transform.OutputSettings);
        transform.Transform(xmlReader, xmlWriter);

        return stringBuilder.ToString();
    }
}
