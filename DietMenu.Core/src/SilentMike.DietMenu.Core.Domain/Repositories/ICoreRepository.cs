namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface ICoreRepository
{
    void SaveIngredients(IEnumerable<CoreIngredientEntity> ingredients);
    void SaveIngredientTypes(IEnumerable<CoreIngredientTypeEntity> ingredientTypes);
    void SaveMealTypes(IEnumerable<CoreMealTypeEntity> mealTypes);
    void SaveCore(CoreEntity core);
}
