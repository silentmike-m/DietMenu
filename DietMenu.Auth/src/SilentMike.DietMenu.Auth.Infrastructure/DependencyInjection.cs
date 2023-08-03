namespace SilentMike.DietMenu.Auth.Infrastructure;

using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Infrastructure.AutoMapper;
using SilentMike.DietMenu.Auth.Infrastructure.Date.Services;
using SilentMike.DietMenu.Auth.Infrastructure.HealthCheck;
using SilentMike.DietMenu.Auth.Infrastructure.Identity;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Interfaces;
using SilentMike.DietMenu.Auth.Infrastructure.IdentityServer;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit;
using SilentMike.DietMenu.Auth.Infrastructure.Swagger;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks(configuration);

        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddIdentity(configuration);

        services.AddIdentityServer4(configuration);

        services.AddMassTransit(configuration);

        services.AddDietMenuSwagger();

        services.AddAutoMapper();

        services.AddSingleton<IDateTimeService, DateTimeService>();
    }

    public static AuthenticationBuilder AddInfrastructure(this AuthenticationBuilder builder)
    {
        builder.AddIdentityServer4Authentication();

        return builder;
    }

    public static void UseInfrastructure(this IApplicationBuilder app, IConfiguration configuration)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("DependencyInjection");

        try
        {
            var context = scope.ServiceProvider.GetRequiredService<DietMenuDbContext>();
            var grantContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
            var systemMigrationService = scope.ServiceProvider.GetRequiredService<ISystemMigrationService>();

            app.UseHealthChecks();

            app.UseDietMenuSwagger();

            app.UseIdentity(context, grantContext, systemMigrationService);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Message}", exception.Message);
        }
    }
}
