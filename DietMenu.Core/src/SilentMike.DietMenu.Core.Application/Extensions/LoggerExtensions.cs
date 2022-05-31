namespace SilentMike.DietMenu.Core.Application.Extensions;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public static class LoggerExtensions
{
    public static IDisposable BeginPropertyScope(this ILogger logger, params ValueTuple<string, object>[] properties)
    {
        var dictionary = properties.ToDictionary(p => p.Item1, p => p.Item2);
        return logger.BeginScope(dictionary);
    }

    public static IDisposable BeginPropertyScope(this ILogger logger, string name, object value)
    {
        return logger.BeginScope(new Dictionary<string, object>
        {
            {name, value},
        });
    }
}
