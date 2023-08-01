namespace SilentMike.DietMenu.Auth.Infrastructure.IdentityServer;

using System.Diagnostics.CodeAnalysis;
using IdentityModel;
using IdentityServer4.Models;

[ExcludeFromCodeCoverage]
internal static class Setup
{
    public static IEnumerable<ApiResource> GetApiResources() =>
        new ApiResource[]
        {
            new("api1", "API")
            {
                UserClaims =
                {
                    JwtClaimTypes.Email,
                },
            },
        };

    public static IEnumerable<ApiScope> GetApiScopes() =>
        new ApiScope[]
        {
            new("api1", "Default API scope"),
        };

    public static IEnumerable<IdentityResource> GetIdentityResources() =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new("user", new[]
            {
                JwtClaimTypes.Email,
            }),
        };
}
