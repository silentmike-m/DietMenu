namespace SilentMike.DietMenu.Mailing.Application;

using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Mailing.Application.Common.Behaviours;
using SilentMike.DietMenu.Mailing.Application.Interfaces;
using SilentMike.DietMenu.Mailing.Application.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddSingleton<IXmlService, XmlService>();

        return services;
    }
}
