namespace SilentMike.DietMenu.Core.Application.Recipes.ViewModels;

using System.Text.Json.Serialization;

public sealed record RecipeIngredient
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = default;
    [JsonPropertyName("ingredient_id")] public Guid IngredientId { get; init; } = default;
    [JsonPropertyName("ingredient_exchanger")] public decimal IngredientExchanger { get; init; } = default;
    [JsonPropertyName("ingredient_name")] public string IngredientName { get; init; } = string.Empty;
    [JsonPropertyName("ingredient_type_id")] public Guid IngredientTypeId { get; init; } = default;
    [JsonPropertyName("ingredient_type_name")] public string IngredientTypeName { get; init; } = string.Empty;
    [JsonPropertyName("quantity")] public int Quantity { get; init; } = default;
    [JsonPropertyName("unit_symbol")] public string UnitSymbol { get; init; } = string.Empty;
}
