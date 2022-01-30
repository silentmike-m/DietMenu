namespace SilentMike.DietMenu.Auth.Infrastructure.IdentityServer;

using IdentityModel;
using IdentityServer4.Models;

internal static class Setup
{
    public static IEnumerable<ApiResource> GetApiResources() =>
        new ApiResource[]
        {
            new ApiResource("api1", "API")
            {
                UserClaims =
                {
                    JwtClaimTypes.Email,
                },
            },
        };

    public static IEnumerable<IdentityResource> GetIdentityResources() =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource(name: "user", userClaims: new[]
            {
                JwtClaimTypes.Email,
            }),
        };

    public static IEnumerable<ApiScope> GetApiScopes() =>
        new ApiScope[]
        {
            new ApiScope(name: "api1", displayName: "Default API scope"),
        };
}
