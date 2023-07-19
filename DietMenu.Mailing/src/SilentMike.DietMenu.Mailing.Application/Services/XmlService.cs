namespace SilentMike.DietMenu.Mailing.Application.Services;

using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using SilentMike.DietMenu.Mailing.Application.Exceptions;
using SilentMike.DietMenu.Mailing.Application.Interfaces;

internal sealed class XmlService : IXmlService
{
    public async Task<string> GetXsltStringAsync(string resourceName, CancellationToken cancellationToken = default)
    {
        await using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

        if (stream is null)
        {
            throw new ResourceNotFoundException(resourceName);
        }

        using var text = new StreamReader(stream);

        return await text.ReadToEndAsync();
    }

    public string TransformToHtml(string xmlString, string xsltString)
    {
        var transform = GetXsltTransform(xsltString);

        using var xmlReader = XmlReader.Create(new StringReader(xmlString));

        var xmlWriterSettings = transform.OutputSettings?.Clone() ?? new XmlWriterSettings();
        xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;

        var result = Transform(transform, xmlString, xmlWriterSettings);

        return result;
    }

    public string TransformToText(string xmlString, string xsltString)
    {
        var transform = GetXsltTransform(xsltString);

        var result = Transform(transform, xmlString, transform.OutputSettings);

        return result;
    }

    private static XslCompiledTransform GetXsltTransform(string xsltString)
    {
        using var stringReader = new StringReader(xsltString);
        using var xsltReader = XmlReader.Create(stringReader);

        var transform = new XslCompiledTransform();
        transform.Load(xsltReader);

        return transform;
    }

    private static string Transform(XslCompiledTransform transform, string xmlString, XmlWriterSettings? xmlWriterSettings)
    {
        using var stringReader = new StringReader(xmlString);
        using var xmlReader = XmlReader.Create(stringReader);

        var stringBuilder = new StringBuilder();
        using var xmlWriter = XmlWriter.Create(new StringWriter(stringBuilder), xmlWriterSettings);

        transform.Transform(xmlReader, xmlWriter);

        return stringBuilder.ToString();
    }
}
