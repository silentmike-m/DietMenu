namespace SilentMike.DietMenu.Core.Application.Recipes.ViewModels;

public sealed record RecipeIngredient
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonPropertyName("ingredient_id")] public Guid IngredientId { get; init; } = Guid.Empty;
    [JsonPropertyName("ingredient_exchanger")] public decimal IngredientExchanger { get; init; } = default;
    [JsonPropertyName("ingredient_name")] public string IngredientName { get; init; } = string.Empty;
    [JsonPropertyName("ingredient_type_id")] public Guid IngredientTypeId { get; init; } = Guid.Empty;
    [JsonPropertyName("ingredient_type_name")] public string IngredientTypeName { get; init; } = string.Empty;
    [JsonPropertyName("quantity")] public int Quantity { get; init; } = default;
    [JsonPropertyName("unit_symbol")] public string UnitSymbol { get; init; } = string.Empty;
}
