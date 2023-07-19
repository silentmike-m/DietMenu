namespace SilentMike.DietMenu.Mailing.Application.Extensions;

using System.Reflection;
using SilentMike.DietMenu.Mailing.Application.Exceptions;

public static class ResourceExtensions
{
    public static async Task<byte[]> GetResourceBytesAsync(this string self, CancellationToken cancellationToken = default)
    {
        await using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(self);

        if (resourceStream is null)
        {
            throw new ResourceNotFoundException(self);
        }

        await using var memoryStream = new MemoryStream();
        await resourceStream.CopyToAsync(memoryStream, cancellationToken);

        return memoryStream.ToArray();
    }
}
