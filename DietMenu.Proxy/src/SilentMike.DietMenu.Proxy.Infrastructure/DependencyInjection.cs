namespace SilentMike.DietMenu.Proxy.Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Proxy.Infrastructure.Common.Interfaces;
using SilentMike.DietMenu.Proxy.Infrastructure.Common.Services;
using SilentMike.DietMenu.Proxy.Infrastructure.IdentityServer4;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeService, DateTimeService>();

        services.AddIdentityServer4(configuration);
    }
}
