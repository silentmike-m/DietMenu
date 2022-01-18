namespace SilentMike.DietMenu.Mailing.Application.Common;

using System.Text;

internal sealed class Utf8StringWriter : StringWriter
{
    public override Encoding Encoding => Encoding.UTF8;
}