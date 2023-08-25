namespace SilentMike.DietMenu.Core.Application.Ingredients.Models;

public sealed record IngredientToImport
{
    public double Exchanger { get; init; } = default;
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = string.Empty;
    public string UnitSymbol { get; init; } = string.Empty;
}
