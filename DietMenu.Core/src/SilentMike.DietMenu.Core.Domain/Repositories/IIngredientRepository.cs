namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IIngredientRepository
{
    Task<IngredientEntity?> Get(Guid ingredientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<IngredientEntity>> GetByFamilyId(Guid familyId, CancellationToken cancellationToken = default);
    Task Save(IEnumerable<IngredientEntity> ingredients, CancellationToken cancellationToken = default);
}
