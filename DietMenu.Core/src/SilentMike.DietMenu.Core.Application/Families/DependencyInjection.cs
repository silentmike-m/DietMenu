namespace SilentMike.DietMenu.Core.Application.Families;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Core.Application.Families.Behaviors;
using SilentMike.DietMenu.Core.Application.Families.Interfaces;
using SilentMike.DietMenu.Core.Application.Families.Processors;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static void AddFamilyMigrationProcess(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(GetFamilyDataToImportBehaviour<,>));

        services.AddTransient(typeof(IGetFamilyDataToImportPostProcessor<,>), typeof(GetFamilyIngredientTypesToImportPostProcessor<,>));
        services.AddTransient(typeof(IGetFamilyDataToImportPostProcessor<,>), typeof(GetFamilyIngredientsToImportPostProcessor<,>));
        services.AddTransient(typeof(IGetFamilyDataToImportPostProcessor<,>), typeof(GetFamilyMealTypesToImportPostProcessor<,>));
    }
}
