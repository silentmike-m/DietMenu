namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Recipes.ViewModels;

internal interface IRecipeReadService
{
    Task<RecipesGrid> GetRecipesGridAsync(GridRequest gridRequest, string? ingredientsFilter, Guid? mealTypeId, Guid userId, CancellationToken cancellationToken = default);
}
