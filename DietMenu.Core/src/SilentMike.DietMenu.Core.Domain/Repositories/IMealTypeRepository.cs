namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IMealTypeRepository
{
    Task<MealTypeEntity?> Get(Guid mealTypeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<MealTypeEntity>> GetByFamilyId(Guid familyId, CancellationToken cancellationToken = default);
    Task Save(IEnumerable<MealTypeEntity> mealTypes, CancellationToken cancellationToken = default);
}
