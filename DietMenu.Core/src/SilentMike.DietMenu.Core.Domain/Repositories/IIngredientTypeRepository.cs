namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IIngredientTypeRepository
{
    Task<IngredientTypeEntity?> Get(Guid familyId, Guid ingredientTypeId, CancellationToken cancellationToken = default);
}
