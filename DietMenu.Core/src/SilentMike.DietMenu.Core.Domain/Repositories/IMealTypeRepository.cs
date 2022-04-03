namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IMealTypeRepository
{
    Task<MealTypeEntity?> GetAsync(Guid familyId, Guid mealTypeId, CancellationToken cancellationToken = default);
}
