namespace SilentMike.DietMenu.Core.Infrastructure.MassTransit;

using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication;
using global::MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit.Families.Consumers;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit.Middlewares;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqOptions = configuration.GetSection(RabbitMqOptions.SECTION_NAME).Get<RabbitMqOptions>();
        rabbitMqOptions ??= new RabbitMqOptions();

        services.AddMassTransit(configure =>
        {
            configure.AddConsumer<CreatedFamilyMessageConsumer>();

            configure.UsingRabbitMq((context, configurator) =>
            {
                configurator.UseConsumeFilter(typeof(ExceptionLoggerFilter<>), context);

                configurator.Host(rabbitMqOptions.HostName, rabbitMqOptions.Port, rabbitMqOptions.VirtualHost, host =>
                {
                    host.Password(rabbitMqOptions.Password);
                    host.Username(rabbitMqOptions.User);

                    if (rabbitMqOptions.UseSsl)
                    {
                        host.UseSsl(ssl => ssl.Protocol = SslProtocols.Tls12 | SslProtocols.Tls13);
                    }
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
