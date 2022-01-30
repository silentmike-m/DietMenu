namespace SilentMike.DietMenu.Core.Infrastructure.MassTransit;

using System.Diagnostics.CodeAnalysis;
using global::MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit.Consumers;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit.Middlewares;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static void AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqOptions = configuration.GetSection(RabbitMqOptions.SectionName).Get<RabbitMqOptions>();

        services.AddMassTransit(configure =>
        {
            configure.AddConsumer<CreatedFamilyMessageConsumer>();

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseConsumeFilter(typeof(ExceptionLoggerFilter<>), context);
                cfg.UseConsumeFilter(typeof(ValidationFilter<>), context);
                cfg.UseConsumeFilter(typeof(RetryFilter<>), context);

                cfg.Host(rabbitMqOptions.Server, host =>
                {
                    host.Username(rabbitMqOptions.User);
                    host.Password(rabbitMqOptions.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddMassTransitHostedService();
    }
}
