namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface ICoreRepository
{
    Task SaveIngredients(IEnumerable<CoreIngredientEntity> ingredients, CancellationToken cancellationToken);
    Task SaveIngredientTypes(IEnumerable<CoreIngredientTypeEntity> ingredientTypes, CancellationToken cancellationToken);
    Task SaveMealTypes(IEnumerable<CoreMealTypeEntity> mealTypes, CancellationToken cancellationToken);
    Task SaveCore(CoreEntity core, CancellationToken cancellationToken);
}
