namespace SilentMike.DietMenu.Core.Application.Families.Models;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Families.Interfaces;
using SilentMike.DietMenu.Core.Domain.Models;

public sealed record FamilyDataToImport : IFamilyDataToImport
{
    public IDictionary<string, ICollection<ApplicationException>> Exceptions { get; init; } = new Dictionary<string, ICollection<ApplicationException>>();
    public Guid FamilyId { get; init; } = Guid.Empty;
    public IReadOnlyList<Ingredient> Ingredients { get; init; } = new List<Ingredient>();
    public string IngredientsVersion { get; set; } = string.Empty;

    public void AddException(string dataName, ApplicationException exception)
    {
        if (!this.Exceptions.ContainsKey(dataName))
        {
            this.Exceptions.Add(dataName, new List<ApplicationException>());
        }

        this.Exceptions[dataName].Add(exception);
    }
}
