namespace DietMenu.Core.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DietMenu.Core.Infrastructure.EntityFramework;
using DietMenu.Core.Infrastructure.HealthChecks;
using DietMenu.Core.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks(configuration);

        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddIdentity(configuration);

        services.AddEntityFramework(configuration);

        return services;
    }

    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        var loggerFactory = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("UseInfrastructure");

        try
        {
            app.UseHealthChecks();

            Core.Infrastructure.Identity.DependencyInjection.UseIdentity(serviceScope);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, exception.Message);
        }
    }
}
