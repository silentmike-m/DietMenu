namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IIngredientRepository
{
    Task<IngredientEntity?> Get(Guid familyId, Guid ingredientId, CancellationToken cancellationToken = default);
    Task Save(IngredientEntity ingredient, CancellationToken cancellationToken = default);
}
