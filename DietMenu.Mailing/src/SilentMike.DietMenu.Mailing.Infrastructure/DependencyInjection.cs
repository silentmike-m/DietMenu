namespace SilentMike.DietMenu.Mailing.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Infrastructure.HealthChecks;
using SilentMike.DietMenu.Mailing.Infrastructure.MassTransit;
using SilentMike.DietMenu.Mailing.Infrastructure.Smtp;
using SilentMike.DietMenu.Mailing.Infrastructure.Swagger;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks(configuration);

        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddMassTransit(configuration);

        services.AddSmtp(configuration);

        services.AddSilentMikeSwagger();
    }

    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        var loggerFactory = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("UseInfrastructure");

        try
        {
            app.UseHealthChecks();

            app.UseSilentMikeSwagger();
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "{Message}", exception.Message);
        }
    }
}
