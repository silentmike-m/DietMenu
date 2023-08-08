namespace SilentMike.DietMenu.Core.Application.Ingredients.Models;

public sealed record IngredientFromExcelFile
{
    public decimal Exchanger { get; init; } = default;
    public string InternalName { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string UnitSymbol { get; init; } = string.Empty;
}
