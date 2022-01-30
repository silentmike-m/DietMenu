namespace SilentMike.DietMenu.Auth.Infrastructure;

using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Infrastructure.HealthCheck;
using SilentMike.DietMenu.Auth.Infrastructure.Identity;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.IdentityServer;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks(configuration);

        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddIdentity(configuration);

        services.AddIdentityServer4(configuration);

        services.AddMassTransit(configuration);
    }

    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("DependencyInjection");

        try
        {
            var context = scope.ServiceProvider.GetRequiredService<DietMenuDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<DietMenuUser>>();

            app.UseHealthChecks();

            app.UseIdentity(context, userManager);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);
        }
    }
}
