namespace SilentMike.DietMenu.Core.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework;
using SilentMike.DietMenu.Core.Infrastructure.HealthChecks;
using SilentMike.DietMenu.Core.Infrastructure.Identity;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks(configuration);

        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddIdentity(configuration);

        services.AddEntityFramework(configuration);

        services.AddMassTransit(configuration);
    }

    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        var loggerFactory = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("UseInfrastructure");

        try
        {
            app.UseHealthChecks();

            SilentMike.DietMenu.Core.Infrastructure.Identity.DependencyInjection.UseIdentity(serviceScope);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, exception.Message);
        }
    }
}
