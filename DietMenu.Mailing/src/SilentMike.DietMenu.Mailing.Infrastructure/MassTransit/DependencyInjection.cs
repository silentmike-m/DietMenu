namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit;

using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication;
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
        var rabbitMqSettings = configuration.GetSection(RabbitMqOptions.SECTION_NAME).Get<RabbitMqOptions>();

        services.AddMassTransit(configure =>
        {
            configure.AddConsumer<EmailDataMessageConsumer>();

            configure.AddRequestClient<IGetFamilyEmailRequest>();

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseConsumeFilter(typeof(ExceptionLoggerFilter<>), context);
                cfg.UseConsumeFilter(typeof(ValidationFilter<>), context);
                cfg.UseConsumeFilter(typeof(RetryFilter<>), context);

                cfg.Host(rabbitMqSettings.HostName, rabbitMqSettings.Port, rabbitMqSettings.VirtualHost, host =>
                {
                    host.Password(rabbitMqSettings.Password);
                    host.Username(rabbitMqSettings.User);

                    if (rabbitMqSettings.UseSsl)
                    {
                        host.UseSsl(ssl => ssl.Protocol = SslProtocols.Tls12);
                    }
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
