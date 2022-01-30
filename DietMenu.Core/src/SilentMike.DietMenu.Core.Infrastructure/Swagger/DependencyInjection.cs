namespace SilentMike.DietMenu.Core.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SilentMike.DietMenu.Core.Infrastructure.Extensions;
using SilentMike.DietMenu.Core.Infrastructure.IdentityServer4;
using SilentMike.DietMenu.Core.Infrastructure.Swagger.Filters;

internal static class DependencyInjection
{
    public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var identityServerOptions = configuration.GetSection(IdentityServer4Options.SectionName).Get<IdentityServer4Options>();

        services.ConfigureSwaggerGen(c =>
        {
            c.CustomSchemaIds(s => s.FullName);
        });

        var authority = new UriBuilder(new Uri(identityServerOptions.Authority));

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Flows = new OpenApiOAuthFlows()
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = authority.GetUriWithPath("/connect/authorize"),
                        TokenUrl = authority.GetUriWithPath("/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "api1", "Default API scope" },
                        },
                    },
                },
                Type = SecuritySchemeType.OAuth2,
            });
            options.OperationFilter<AuthorizeCheckOperationFilter>();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DietMenu Core WebApi",
                Version = "v1",
            });
        });
    }

    public static void UseDietMenuSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(options =>
        {
            options.PreSerializeFilters.Add((swagger, httpReq) =>
            {
                if (httpReq.Headers.ContainsKey("X-Forwarded-Host"))
                {
                    var basePath = "api";
                    var serverUrl = $"https://{httpReq.Headers["X-Forwarded-Host"]}/{basePath}";
                    swagger.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer
                        {
                            Url = serverUrl,
                        },
                    };
                }
            });
        });

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("v1/swagger.json", "DietMenu Core WebApi v1");
            options.OAuthClientId("swagger");
            options.OAuthUsePkce();
        });
    }
}
