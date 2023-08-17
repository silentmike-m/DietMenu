namespace SilentMike.DietMenu.Core.Infrastructure.EPPlus.Extensions;

internal static class EpplusExtensions
{
    public static double ToDouble(this object? self, double defaultValue)
    {
        if (self is null)
        {
            return defaultValue;
        }

        var text = self.ToString();

        if (double.TryParse(text, out var value))
        {
            return value;
        }

        return defaultValue;
    }

    public static string ToEmptyString(this object? self)
        => self?.ToString() ?? string.Empty;

    public static Guid ToNonEmptyGuid(this object? self)
    {
        var guidString = self?.ToString();

        return Guid.TryParse(guidString, out var guid)
            ? guid
            : Guid.NewGuid();
    }
}
