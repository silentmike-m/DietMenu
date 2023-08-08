namespace SilentMike.DietMenu.Core.Infrastructure.EPPlus.Extensions;

internal static class EpplusExtensions
{
    public static decimal ToDecimal(this object? self, decimal defaultValue)
    {
        if (self is null)
        {
            return defaultValue;
        }

        var text = self.ToString();

        if (decimal.TryParse(text, out var value))
        {
            return value;
        }

        return defaultValue;
    }
}
