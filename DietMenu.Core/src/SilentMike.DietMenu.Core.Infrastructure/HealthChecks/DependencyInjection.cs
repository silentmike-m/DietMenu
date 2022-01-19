namespace SilentMike.DietMenu.Core.Infrastructure.HealthChecks;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework;
using SilentMike.DietMenu.Core.Infrastructure.HealthChecks.Models;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");
        var rabbitMqOptions = configuration.GetSection(RabbitMqOptions.SectionName).Get<RabbitMqOptions>();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>(name: "Identity")
            .AddRabbitMQ(_ => new ConnectionFactory
            {
                HostName = rabbitMqOptions.HostName,
                Password = rabbitMqOptions.Password,
                UserName = rabbitMqOptions.User,
            }, "RabbitMQ")
            .AddSqlServer(connectionString: defaultConnectionString, name: "SQL Default")
            ;
    }

    public static void UseHealthChecks(this IApplicationBuilder app)
    {
        var healthOptions = new HealthCheckOptions
        {
            ResponseWriter = HealthCheckResponseWriter,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status200OK,
            },
        };

        app.UseHealthChecks("/health", healthOptions);
    }

    private static async Task HealthCheckResponseWriter(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var componentHealthChecks = new List<ComponentHealthCheck>();

        foreach (var (component, componentReport) in report.Entries)
        {
            var componentHealthCheck = new ComponentHealthCheck
            {
                Component = component,
                Description = componentReport.Description,
                ErrorMessage = componentReport.Exception?.Message,
                HealthCheckDurationInMilliseconds = componentReport.Duration.Milliseconds,
                Status = componentReport.Status.ToString(),
            };
            componentHealthChecks.Add(componentHealthCheck);
        }

        var healthCheck = new HealthCheck
        {
            HealthChecks = componentHealthChecks,
            HealthCheckDurationInMilliseconds = report.TotalDuration.Milliseconds,
            Status = report.Status.ToString(),
        };

        var jsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(healthCheck, jsonOptions));
    }
}
