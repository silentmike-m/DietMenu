namespace SilentMike.DietMenu.Core.Infrastructure.Common.Extensions;

public static class MemoryStreamExtensions
{
    public static byte[] ReadAllBytes(this Stream stream)
    {
        using var memoryStream = new MemoryStream();

        var buffer = new byte[4096];

        for (var count = stream.Read(buffer, offset: 0, count: 4096); count > 0; count = stream.Read(buffer, offset: 0, count: 4096))
        {
            memoryStream.Write(buffer, offset: 0, count);
        }

        return memoryStream.ToArray();
    }
}
