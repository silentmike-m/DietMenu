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
using SilentMike.DietMenu.Core.Infrastructure.Swagger;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks(configuration);

        services.AddSwagger();

        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddDietMenuIdentity(configuration);

        services.AddEntityFramework(configuration);

        services.AddMassTransit(configuration);
    }

    public static void UseInfrastructure(this IApplicationBuilder app, IConfiguration configuration)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        var loggerFactory = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("UseInfrastructure");

        try
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            app.UseHealthChecks();

            app.UseSwagger(configuration);

            Identity.DependencyInjection.UseDietMenuIdentity(context);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, exception.Message);
        }
    }
}
