namespace SilentMike.DietMenu.Core.Application.Core.Models;

using SilentMike.DietMenu.Core.Application.Core.Interfaces;
using SilentMike.DietMenu.Core.Domain.Entities;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

public sealed record CoreDataToImport : ICoreDataToImport
{
    public CoreEntity Core { get; init; } = new(Guid.NewGuid());
    public IDictionary<string, ICollection<ApplicationException>> Exceptions { get; init; } = new Dictionary<string, ICollection<ApplicationException>>();
    public ICollection<CoreIngredientTypeEntity> IngredientTypes { get; init; } = new List<CoreIngredientTypeEntity>();
    public ICollection<CoreIngredientEntity> Ingredients { get; init; } = new List<CoreIngredientEntity>();
    public ICollection<CoreMealTypeEntity> MealTypes { get; init; } = new List<CoreMealTypeEntity>();

    public void AddException(string area, ApplicationException exception)
    {
        if (!this.Exceptions.ContainsKey(area))
        {
            this.Exceptions.Add(area, new List<ApplicationException>());
        }

        this.Exceptions[area].Add(exception);
    }
}
