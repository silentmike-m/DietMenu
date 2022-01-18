namespace SilentMike.DietMenu.Mailing.Application.Extensions;

internal static class ByteArrayExtensions
{
    public static string GetString(this byte[] data)
    {
        using var stream = new MemoryStream(data);
        using var streamReader = new StreamReader(stream);
        return streamReader.ReadToEnd();
    }
}
