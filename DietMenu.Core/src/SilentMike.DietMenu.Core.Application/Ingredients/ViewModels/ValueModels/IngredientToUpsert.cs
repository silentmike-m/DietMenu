namespace SilentMike.DietMenu.Core.Application.Ingredients.ViewModels.ValueModels;

using System.Text.Json.Serialization;

public sealed record IngredientToUpsert
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonPropertyName("exchanger")] public decimal? Exchanger { get; init; } = 1;
    [JsonPropertyName("name")] public string? Name { get; init; } = default;
    [JsonPropertyName("type_id")] public Guid? TypeId { get; init; } = default;
    [JsonPropertyName("unit_symbol")] public string? UnitSymbol { get; init; } = default;
}
