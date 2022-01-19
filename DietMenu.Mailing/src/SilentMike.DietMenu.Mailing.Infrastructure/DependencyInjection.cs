﻿namespace SilentMike.DietMenu.Mailing.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Infrastructure.HealthChecks;
using SilentMike.DietMenu.Mailing.Infrastructure.MassTransit;
using SilentMike.DietMenu.Mailing.Infrastructure.Smtp;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks(configuration);

        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddMassTransit(configuration);

        services.AddSmtp(configuration);
    }

    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        var loggerFactory = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("UseInfrastructure");

        try
        {
            app.UseHealthChecks();
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, exception.Message);
        }
    }
}
