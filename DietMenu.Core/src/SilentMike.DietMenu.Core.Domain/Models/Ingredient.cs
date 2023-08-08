namespace SilentMike.DietMenu.Core.Domain.Models;

using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Domain.Extensions;

public sealed class Ingredient
{
    public decimal Exchanger { get; private set; } = default;
    public Guid FamilyId { get; private set; }
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string UnitSymbol { get; private set; }

    public Ingredient(Guid id, decimal exchanger, Guid familyId, string name, string type, string unitSymbol)
    {
        this.Id = id;
        this.FamilyId = familyId;
        this.UnitSymbol = unitSymbol;

        this.SetExchanger(exchanger);
        this.SetName(name);
        this.SetType(type);
    }

    public void SetExchanger(decimal exchanger)
    {
        if (exchanger < 0)
        {
            throw new IngredientInvalidExchangerException(this.Id, exchanger);
        }

        this.Exchanger = exchanger;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new IngredientEmptyNameException(this.Id);
        }

        this.Name = name;
    }

    private void SetType(string type)
    {
        var typeName = IngredientTypeNames.IngredientTypes.GetIgnoreCase(type);

        if (typeName is null)
        {
            throw new IngredientInvalidTypeException(this.Id, type);
        }

        this.Type = typeName;
    }
}
