namespace SilentMike.DietMenu.Core.Application.Families.Models;

using SilentMike.DietMenu.Core.Application.Families.Interfaces;
using SilentMike.DietMenu.Core.Domain.Entities;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

public sealed record FamilyDataToImport : IFamilyDataToImport
{
    public IDictionary<string, ICollection<ApplicationException>> Exceptions { get; init; } = new Dictionary<string, ICollection<ApplicationException>>();
    public ICollection<IngredientTypeEntity> IngredientTypes { get; init; } = new List<IngredientTypeEntity>();
    public ICollection<IngredientEntity> Ingredients { get; init; } = new List<IngredientEntity>();
    public ICollection<MealTypeEntity> MealTypes { get; init; } = new List<MealTypeEntity>();

    public void AddException(string dataName, ApplicationException exception)
    {
        if (!this.Exceptions.ContainsKey(dataName))
        {
            this.Exceptions.Add(dataName, new List<ApplicationException>());
        }

        this.Exceptions[dataName].Add(exception);
    }
}
