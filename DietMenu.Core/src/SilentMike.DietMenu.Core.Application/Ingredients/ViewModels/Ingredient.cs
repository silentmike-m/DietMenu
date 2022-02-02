namespace SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;

using System.Text.Json.Serialization;

public sealed record Ingredient
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonPropertyName("exchanger")] public decimal Exchanger { get; init; } = 1;
    [JsonPropertyName("is_system")] public bool IsSystem { get; init; } = default;
    [JsonPropertyName("name")] public string Name { get; init; } = string.Empty;
    [JsonPropertyName("type_id")] public Guid TypeId { get; init; } = Guid.Empty;
    [JsonPropertyName("type_name")] public string TypeName { get; init; } = string.Empty;
    [JsonPropertyName("unit_symbol")] public string UnitSymbol { get; init; } = string.Empty;
}
