namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface ICoreRepository
{
    Task SaveIngredientsAsync(IEnumerable<CoreIngredientEntity> ingredients, CancellationToken cancellationToken = default);
    Task SaveIngredientTypesAsync(IEnumerable<CoreIngredientTypeEntity> ingredientTypes, CancellationToken cancellationToken = default);
    Task SaveMealTypesAsync(IEnumerable<CoreMealTypeEntity> mealTypes, CancellationToken cancellationToken = default);
    Task SaveCoreAsync(CoreEntity core, CancellationToken cancellationToken = default);
}
