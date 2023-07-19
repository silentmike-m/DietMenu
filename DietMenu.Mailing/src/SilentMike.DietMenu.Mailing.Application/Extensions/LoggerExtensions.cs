namespace SilentMike.DietMenu.Mailing.Application.Extensions;

using Microsoft.Extensions.Logging;

public static class LoggerExtensions
{
    public static IDisposable BeginPropertyScope(this ILogger logger, params ValueTuple<string, object>[] properties)
    {
        var dictionary = properties.ToDictionary(p => p.Item1, p => p.Item2);

        return logger.BeginScope(dictionary);
    }
}
