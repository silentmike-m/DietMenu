namespace SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;

using System.Text.Json.Serialization;

public sealed record Replacement
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = default;
    [JsonPropertyName("exchanger")] public decimal Exchanger { get; init; } = default;
    [JsonPropertyName("name")] public string Name { get; init; } = string.Empty;
    [JsonPropertyName("quantity")] public decimal Quantity { get; init; } = default;
    [JsonPropertyName("unit_symbol")] public string UnitSymbol { get; init; } = string.Empty;
}
