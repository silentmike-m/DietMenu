namespace SilentMike.DietMenu.Core.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;
using SilentMike.DietMenu.Core.Infrastructure.Hangfire;
using SilentMike.DietMenu.Core.Infrastructure.HealthChecks;
using SilentMike.DietMenu.Core.Infrastructure.IdentityServer4;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit;
using SilentMike.DietMenu.Core.Infrastructure.Swagger;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string hangFireServerName)
    {
        services.AddHealthChecks(configuration);

        services.AddSwagger(configuration);

        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddSingleton<IFileProvider>(new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly()));

        services.AddIdentityServer4(configuration);

        services.AddEntityFramework(configuration);

        services.AddMassTransit(configuration);

        services.AddHangfire(configuration, hangFireServerName);
    }

    public static void UseInfrastructure(this IApplicationBuilder app, string hangFireServerName)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        var loggerFactory = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("UseInfrastructure");

        try
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<DietMenuDbContext>();

            app.UseHealthChecks();

            app.UseDietMenuSwagger();

            app.UseEntityFramework(context);

            app.UseHangfire(hangFireServerName);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "{Message}", exception.Message);
        }
    }
}
