namespace SilentMike.DietMenu.Core.Infrastructure.Common.Extensions;

internal static class UriBuilderExtensions
{
    public static Uri GetUriWithPath(this UriBuilder self, string path)
    {
        var builder = new UriBuilder(self.Uri)
        {
            Path = path,
        };

        return builder.Uri;
    }
}
