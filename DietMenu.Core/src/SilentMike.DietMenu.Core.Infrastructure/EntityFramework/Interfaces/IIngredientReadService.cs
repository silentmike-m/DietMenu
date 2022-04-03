namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;

internal interface IIngredientReadService
{
    Task<IngredientsGrid> GetIngredientsGridAsync(Guid familyId, GridRequest gridRequest, Guid? typeId, CancellationToken cancellationToken = default);
}
