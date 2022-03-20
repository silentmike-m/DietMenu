namespace SilentMike.DietMenu.Core.Application.Families.Interfaces;

using SilentMike.DietMenu.Core.Domain.Entities;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

internal interface IFamilyDataToImport
{
    IDictionary<string, ICollection<ApplicationException>> Exceptions { get; }
    ICollection<IngredientTypeEntity> IngredientTypes { get; }
    ICollection<IngredientEntity> Ingredients { get; }
    ICollection<MealTypeEntity> MealTypes { get; }

    void AddException(string dataName, ApplicationException exception);
}
