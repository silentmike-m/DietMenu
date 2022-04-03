namespace SilentMike.DietMenu.Auth.Application;

using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Auth.Application.Common.Behaviours;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
}
