namespace SilentMike.DietMenu.Core.Application.Ingredients.Models;

public sealed record IngredientToCreate
{
    public decimal Exchanger { get; init; } = default;
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string? UnitSymbol { get; init; } = default;
}
