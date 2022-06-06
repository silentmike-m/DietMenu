namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IIngredientRepository : IRepository
{
    IngredientEntity? Get(Guid familyId, Guid ingredientId);
    void Save(IngredientEntity ingredient);
}
