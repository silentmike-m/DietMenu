namespace SilentMike.DietMenu.Core.Application.Families.Interfaces;

using SilentMike.DietMenu.Core.Domain.Entities;

internal interface IGetFamilyDataToImport
{
    CoreEntity Core { get; }
    IEnumerable<CoreIngredientTypeEntity> CoreIngredientTypes { get; }
    IEnumerable<CoreIngredientEntity> CoreIngredients { get; }
    IEnumerable<CoreMealTypeEntity> CoreMealTypes { get; }
    FamilyEntity Family { get; }
}
