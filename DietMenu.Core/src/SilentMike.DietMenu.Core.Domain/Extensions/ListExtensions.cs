namespace SilentMike.DietMenu.Core.Domain.Extensions;

public static class ListExtensions
{
    public static string? GetIgnoreCase(this IEnumerable<string> self, string value)
        => self.SingleOrDefault(s => string.Equals(s, value, StringComparison.InvariantCultureIgnoreCase));
}
