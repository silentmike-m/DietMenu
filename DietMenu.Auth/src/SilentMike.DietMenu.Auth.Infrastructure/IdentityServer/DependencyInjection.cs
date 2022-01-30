namespace SilentMike.DietMenu.Auth.Infrastructure.IdentityServer;

using IdentityServer4.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.IdentityServer.Services;

internal static class DependencyInjection
{
    public static void AddIdentityServer4(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentityServer()
            .AddAspNetIdentity<DietMenuUser>()
            .AddDeveloperSigningCredential()
            .AddInMemoryApiResources(Setup.GetApiResources())
            .AddInMemoryApiScopes(Setup.GetApiScopes())
            .AddInMemoryClients(configuration.GetSection("IdentityServer:Clients"))
            .AddInMemoryIdentityResources(Setup.GetIdentityResources())
            .AddInMemoryPersistedGrants()
            .AddProfileService<ProfileService>();

        services.AddTransient<IProfileService, ProfileService>();
    }
}
