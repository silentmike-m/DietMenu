namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit;

using global::MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Consumers;

internal static class DependencyInjection
{
    public static void AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqOptions = configuration.GetSection(RabbitMqOptions.SectionName).Get<RabbitMqOptions>();

        services.AddMassTransit(configure =>
        {
            configure.AddConsumer<GetSystemUserEmailRequestConsumer>();
            configure.AddConsumer<GetFamilyUserEmailRequestConsumer>();

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqOptions.Server, host =>
                {
                    host.Password(rabbitMqOptions.Password);
                    host.Username(rabbitMqOptions.User);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddMassTransitHostedService();
    }
}
