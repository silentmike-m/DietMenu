namespace SilentMike.DietMenu.Core.Application.Ingredients.Models;

public sealed class IngredientToImport
{
    public decimal Exchanger { get; init; } = 1;
    public string InternalName { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string UnitSymbol { get; init; } = string.Empty;
}
