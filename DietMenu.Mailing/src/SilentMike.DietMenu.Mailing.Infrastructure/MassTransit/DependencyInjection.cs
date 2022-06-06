namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit;

using System.Diagnostics.CodeAnalysis;
using global::MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Consumers;
using SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Middlewares;
using SilentMike.DietMenu.Shared.Identity.Interfaces;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static void AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqOptions = configuration.GetSection(RabbitMqOptions.SectionName).Get<RabbitMqOptions>();

        services.AddMassTransit(configure =>
        {
            configure.AddConsumer<EmailDataMessageConsumer>();

            configure.AddRequestClient<IIdentityDataRequest>();

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
