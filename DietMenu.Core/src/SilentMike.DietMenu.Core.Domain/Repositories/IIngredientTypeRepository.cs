namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IIngredientTypeRepository
{
    Task<IngredientTypeEntity?> Get(Guid ingredientTypeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<IngredientTypeEntity>> GetByFamilyId(Guid familyId, CancellationToken cancellationToken = default);
    Task Save(IEnumerable<IngredientTypeEntity> ingredientTypes, CancellationToken cancellationToken = default);
    Task Save(IngredientTypeEntity ingredientType, CancellationToken cancellationToken = default);
}
