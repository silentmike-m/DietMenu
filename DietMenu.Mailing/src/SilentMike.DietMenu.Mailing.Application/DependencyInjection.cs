namespace SilentMike.DietMenu.Mailing.Application;

using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Mailing.Application.Common.Behaviours;
using SilentMike.DietMenu.Mailing.Application.Emails.Services;
using SilentMike.DietMenu.Mailing.Application.Interfaces;
using SilentMike.DietMenu.Mailing.Application.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

        services.AddSingleton<EmailFactory>();
        services.AddSingleton<IXmlService, XmlService>();

        return services;
    }
}
