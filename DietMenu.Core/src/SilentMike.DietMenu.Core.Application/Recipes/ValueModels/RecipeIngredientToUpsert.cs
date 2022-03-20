namespace SilentMike.DietMenu.Core.Application.Recipes.ValueModels;

using System.Text.Json.Serialization;

public sealed record RecipeIngredientToUpsert
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonPropertyName("ingredient_id")] public Guid IngredientId { get; init; } = Guid.Empty;
    [JsonPropertyName("quantity")] public int Quantity { get; init; } = default;
}
