namespace SilentMike.DietMenu.Core.Application.Common.Extensions;

public static class StringExtensions
{
    public static bool IsInvariantCultureIgnoreCaseEquals(this string self, string other)
        => string.Equals(self, other, StringComparison.InvariantCultureIgnoreCase);
}
