namespace SilentMike.DietMenu.Auth.Application.Common.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ToTrimmedDateTime(this DateTime source) => new(source.Year, source.Month, source.Day, source.Hour, source.Minute, source.Second, source.Kind);
}
