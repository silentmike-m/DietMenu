namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IIngredientRepository
{
    Task<IngredientEntity?> GetAsync(Guid familyId, Guid ingredientId, CancellationToken cancellationToken = default);
    Task SaveAsync(IngredientEntity ingredient, CancellationToken cancellationToken = default);
}
