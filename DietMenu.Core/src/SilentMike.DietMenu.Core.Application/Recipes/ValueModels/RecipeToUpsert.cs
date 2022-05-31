namespace SilentMike.DietMenu.Core.Application.Recipes.ValueModels;

public sealed record RecipeToUpsert
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonPropertyName("carbohydrates")] public int? Carbohydrates { get; init; } = default;
    [JsonPropertyName("description")] public string? Description { get; set; } = default;
    [JsonPropertyName("energy")] public int? Energy { get; init; } = default;
    [JsonPropertyName("fat")] public int? Fat { get; init; } = default;
    [JsonPropertyName("ingredients")] public IReadOnlyList<RecipeIngredientToUpsert> Ingredients { get; init; } = new List<RecipeIngredientToUpsert>().AsReadOnly();
    [JsonPropertyName("meal_type_id")] public Guid? MealTypeId { get; init; } = default;
    [JsonPropertyName("name")] public string? Name { get; init; } = default;
    [JsonPropertyName("protein")] public int? Protein { get; init; } = default;
}
