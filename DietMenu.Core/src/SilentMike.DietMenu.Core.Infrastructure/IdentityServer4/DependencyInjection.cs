namespace SilentMike.DietMenu.Core.Infrastructure.IdentityServer4;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static void AddIdentityServer4(this IServiceCollection services, IConfiguration configuration)
    {
        var identityServerOptions = configuration.GetSection(IdentityServer4Options.SectionName).Get<IdentityServer4Options>();

        services.Configure<IdentityServer4Options>(configuration.GetSection(IdentityServer4Options.SectionName));

        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = identityServerOptions.Authority;
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
        });
    }
}
