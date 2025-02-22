﻿namespace SilentMike.DietMenu.Core.Infrastructure.IdentityServer4;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SilentMike.DietMenu.Core.Application.Shared;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static void AddIdentityServer4(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityServer4Options>(configuration.GetSection(IdentityServer4Options.SECTION_NAME));

        var identityServerOptions = configuration.GetSection(IdentityServer4Options.SECTION_NAME).Get<IdentityServer4Options>();
        identityServerOptions ??= new IdentityServer4Options();

        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = identityServerOptions.Authority;
                //If this is set to true, the Type is set to the JSON claim 'name' after translating using this mapping. Otherwise, no mapping occurs. True by default.
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "api1");
            });

            options.AddPolicy("Hangfire", policy =>
            {
                policy.AddRequirements().RequireAuthenticatedUser();
                policy.RequireRole(DietMenuRoleNames.SYSTEM);
            });
        });
    }
}
