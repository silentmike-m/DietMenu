namespace SilentMike.DietMenu.Mailing.Infrastructure.Swagger;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

public static class DependencyInjection
{
    public static void AddSilentMikeSwagger(this IServiceCollection services)
    {
        services.ConfigureSwaggerGen(c =>
        {
            c.CustomSchemaIds(s => s.FullName);
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DietMenu Mailing WebApi",
                Version = "v1",
            });
        });
    }

    public static void UseSilentMikeSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(options =>
        {
            options.PreSerializeFilters.Add((swagger, httpReq) =>
            {
                if (!httpReq.Headers.ContainsKey("X-Forwarded-Host"))
                {
                    return;
                }

                var basePath = "api";
                var serverUrl = $"https://{httpReq.Headers["X-Forwarded-Host"]}/{basePath}";

                swagger.Servers = new List<OpenApiServer>
                {
                    new()
                    {
                        Url = serverUrl,
                    },
                };
            });
        });

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("v1/swagger.json", "DietMenu Mailing WebApi v1");
            options.OAuthClientId("swagger");
            options.OAuthUsePkce();
        });
    }
}
