namespace SilentMike.DietMenu.Auth.Infrastructure.Swagger;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Auth.Infrastructure.Swagger.Filters;

internal static class DependencyInjection
{
    public static void AddDietMenuSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();
    }

    public static void UseDietMenuSwagger(this IApplicationBuilder app)
    {
        app.UseMiddleware<SwaggerAuthorizationMiddleware>();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
