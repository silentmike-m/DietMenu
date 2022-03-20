namespace SilentMike.DietMenu.Core.Application;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Core.Application.Common.Behaviours;
using SilentMike.DietMenu.Core.Application.Core;
using SilentMike.DietMenu.Core.Application.Families;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddCoreMigrationProcess();
        services.AddFamilyMigrationProcess();
    }
}
