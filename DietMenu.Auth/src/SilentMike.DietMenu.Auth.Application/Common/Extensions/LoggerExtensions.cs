namespace SilentMike.DietMenu.Auth.Application.Common.Extensions;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

[ExcludeFromCodeCoverage]
public static class LoggerExtensions
{
    public static IDisposable BeginPropertyScope(this ILogger logger, params ValueTuple<string, object>[] properties)
    {
        var dictionary = properties.ToDictionary(p => p.Item1, p => p.Item2);

        return logger.BeginScope(dictionary)!;
    }
}
