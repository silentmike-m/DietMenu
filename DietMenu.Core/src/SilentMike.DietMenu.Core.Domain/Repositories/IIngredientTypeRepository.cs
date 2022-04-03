namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IIngredientTypeRepository
{
    Task<IngredientTypeEntity?> GetAsync(Guid familyId, Guid ingredientTypeId, CancellationToken cancellationToken = default);
}
