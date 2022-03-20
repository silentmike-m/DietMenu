namespace SilentMike.DietMenu.Core.Application.Core;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Core.Application.Core.Behaviours;
using SilentMike.DietMenu.Core.Application.Core.Interfaces;
using SilentMike.DietMenu.Core.Application.Core.PostProcessors;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static void AddCoreMigrationProcess(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(GetCoreDataToImportBehaviour<,>));

        services.AddTransient(typeof(IGetCoreDataToImportPostProcessor<,>), typeof(GetCoreIngredientTypesToImportPostProcessor<,>));
        services.AddTransient(typeof(IGetCoreDataToImportPostProcessor<,>), typeof(GetCoreIngredientsToImportPostProcessor<,>));
        services.AddTransient(typeof(IGetCoreDataToImportPostProcessor<,>), typeof(GetCoreMealTypesToImportPostProcessor<,>));
    }
}
