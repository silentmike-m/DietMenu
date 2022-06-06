namespace SilentMike.DietMenu.Core.Application.Recipes.ViewModels;

public sealed record RecipesGrid
{
    [JsonPropertyName("count")] public int Count { get; init; } = default;
    [JsonPropertyName("elements")] public IReadOnlyList<Recipe> Elements { get; init; } = new List<Recipe>().AsReadOnly();
}
