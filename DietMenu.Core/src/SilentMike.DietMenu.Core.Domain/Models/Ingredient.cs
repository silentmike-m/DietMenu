namespace SilentMike.DietMenu.Core.Domain.Models;

using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Domain.Extensions;

public sealed class Ingredient : BusinessModel
{
    public double Exchanger { get; private set; } = default;
    public Guid FamilyId { get; private set; }
    private Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string UnitSymbol { get; private set; }

    public Ingredient(double exchanger, Guid familyId, string name, string type, string unitSymbol)
    {
        this.FamilyId = familyId;
        this.UnitSymbol = unitSymbol;

        this.SetExchanger(exchanger);
        this.SetName(name);
        this.SetType(type);

        this.MarkOld();
    }

    public void SetExchanger(double exchanger)
    {
        if (exchanger < 0)
        {
            throw new IngredientInvalidExchangerException(this.Id, exchanger);
        }

        this.Exchanger = exchanger;
        this.MarkDirty();
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new IngredientEmptyNameException(this.Id);
        }

        this.Name = name;
        this.MarkDirty();
    }

    private void SetType(string type)
    {
        var typeName = IngredientTypeNames.IngredientTypes.GetIgnoreCase(type);

        if (typeName is null)
        {
            throw new IngredientInvalidTypeException(this.Id, type);
        }

        this.Type = typeName;
        this.MarkDirty();
    }
}
