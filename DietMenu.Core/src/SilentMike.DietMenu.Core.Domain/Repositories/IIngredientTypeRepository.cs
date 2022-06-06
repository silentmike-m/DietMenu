namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IIngredientTypeRepository
{
    IngredientTypeEntity? Get(Guid familyId, Guid ingredientTypeId);
}
