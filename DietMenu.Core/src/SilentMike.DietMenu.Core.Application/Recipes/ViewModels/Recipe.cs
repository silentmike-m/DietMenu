namespace SilentMike.DietMenu.Core.Application.Recipes.ViewModels;

using System.Text.Json.Serialization;

public sealed record Recipe
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = default;
    [JsonPropertyName("carbohydrates")] public decimal Carbohydrates { get; init; } = default;
    [JsonPropertyName("description")] public string Description { get; init; } = string.Empty;
    [JsonPropertyName("energy")] public decimal Energy { get; init; } = default;
    [JsonPropertyName("fat")] public decimal Fat { get; init; } = default;
    [JsonPropertyName("ingredients")] public IReadOnlyList<RecipeIngredient> Ingredients { get; init; } = new List<RecipeIngredient>().AsReadOnly();
    [JsonPropertyName("meal_type_id")] public Guid MealTypeId { get; init; } = default;
    [JsonPropertyName("meal_type_name")] public string MealTypeName { get; init; } = string.Empty;
    [JsonPropertyName("name")] public string Name { get; init; } = string.Empty;
    [JsonPropertyName("protein")] public int Protein { get; init; } = default;
}
