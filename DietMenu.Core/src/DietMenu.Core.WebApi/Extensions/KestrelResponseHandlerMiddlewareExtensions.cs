namespace DietMenu.Core.WebApi.Extensions;

using DietMenu.Core.WebApi.Middlewares;

internal static class KestrelResponseHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseKestrelResponseHandlerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<KestrelResponseHandlerMiddleware>();
    }
}
