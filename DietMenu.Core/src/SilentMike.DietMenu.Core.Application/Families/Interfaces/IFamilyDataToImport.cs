namespace SilentMike.DietMenu.Core.Application.Families.Interfaces;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Domain.Entities;

internal interface IFamilyDataToImport
{
    IDictionary<string, ICollection<ApplicationException>> Exceptions { get; }
    ICollection<IngredientEntity> Ingredients { get; }

    void AddException(string dataName, ApplicationException exception);
}
