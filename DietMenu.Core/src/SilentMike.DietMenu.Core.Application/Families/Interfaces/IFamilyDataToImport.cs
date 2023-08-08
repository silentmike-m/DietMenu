namespace SilentMike.DietMenu.Core.Application.Families.Interfaces;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Domain.Models;

internal interface IFamilyDataToImport
{
    IDictionary<string, ICollection<ApplicationException>> Exceptions { get; }
    Guid FamilyId { get; }
    IReadOnlyList<Ingredient> Ingredients { get; }

    void AddException(string dataName, ApplicationException exception);
}
