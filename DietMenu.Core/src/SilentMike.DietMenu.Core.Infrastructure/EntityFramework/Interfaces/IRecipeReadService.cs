namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Recipes.ViewModels;

internal interface IRecipeReadService
{
    Task<RecipesGrid> GetRecipesGrid(GridRequest gridRequest, string? ingredientFilter, Guid? mealTypeId, Guid userId);
}
