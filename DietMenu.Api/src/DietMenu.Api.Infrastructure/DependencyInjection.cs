namespace DietMenu.Api.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DietMenu.Api.Infrastructure.EntityFramework;
using DietMenu.Api.Infrastructure.HealthChecks;
using DietMenu.Api.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks(configuration);

        var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddIdentity(configuration);

        services.AddScoped<ApplicationDbContext>();

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(defaultConnectionString));

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

            Identity.DependencyInjection.UseIdentity(serviceScope);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, exception.Message);
        }
    }
}
