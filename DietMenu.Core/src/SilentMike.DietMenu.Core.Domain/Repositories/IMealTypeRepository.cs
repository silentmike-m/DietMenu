namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IMealTypeRepository
{
    MealTypeEntity? Get(Guid familyId, Guid mealTypeId);
}
