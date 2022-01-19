namespace SilentMike.DietMenu.Core.Infrastructure.Swagger;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(s => s.FullName);
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DietMenu Core WebApi",
                Version = "v1",
            });
        });
    }

    public static void UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "DietMenu Core WebApi v1"));
    }
}
