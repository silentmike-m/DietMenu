namespace SilentMike.DietMenu.Core.Infrastructure.EPPlus.Extensions;

internal static class EpplusExtensions
{
    public static decimal ToDecimal(this object? self)
    {
        if (self is null)
        {
            return 1;
        }

        var text = self.ToString();
        if (decimal.TryParse(text, out var value))
        {
            return value;
        }

        return 1;
    }
}
