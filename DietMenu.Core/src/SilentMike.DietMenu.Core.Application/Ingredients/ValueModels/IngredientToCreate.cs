namespace SilentMike.DietMenu.Core.Application.Ingredients.ValueModels;

public sealed record IngredientToCreate
{
    [JsonPropertyName("exchanger")] public decimal Exchanger { get; init; } = default;
    [JsonPropertyName("name")] public string Name { get; init; } = string.Empty;
    [JsonPropertyName("type")] public string Type { get; init; } = string.Empty;
    [JsonPropertyName("unit_symbol")] public string? UnitSymbol { get; init; } = default;
}
