namespace SilentMike.DietMenu.Mailing.Application.Extensions;

using System.Xml.Serialization;
using SilentMike.DietMenu.Mailing.Application.Common;

internal static class XmlSerializerExtensions
{
    public static string Serialize(this XmlSerializer self, object o)
    {
        var ns = new XmlSerializerNamespaces();
        ns.Add("", "");

        using var stringWriter = new Utf8StringWriter();
        self.Serialize(stringWriter, o, ns);

        return stringWriter.ToString();
    }
}
