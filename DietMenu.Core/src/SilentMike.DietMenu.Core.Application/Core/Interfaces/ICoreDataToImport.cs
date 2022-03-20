namespace SilentMike.DietMenu.Core.Application.Core.Interfaces;

using SilentMike.DietMenu.Core.Domain.Entities;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

internal interface ICoreDataToImport
{
    CoreEntity Core { get; }
    IDictionary<string, ICollection<ApplicationException>> Exceptions { get; }
    ICollection<CoreIngredientTypeEntity> IngredientTypes { get; }
    ICollection<CoreIngredientEntity> Ingredients { get; }
    ICollection<CoreMealTypeEntity> MealTypes { get; }

    void AddException(string area, ApplicationException exception);
}
