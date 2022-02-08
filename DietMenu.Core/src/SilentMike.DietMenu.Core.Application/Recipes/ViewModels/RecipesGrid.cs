namespace SilentMike.DietMenu.Core.Application.Recipes.ViewModels;

using System.Text.Json.Serialization;

public sealed record RecipesGrid
{
    [JsonPropertyName("count")] public int Count { get; init; } = default;
    [JsonPropertyName("elements")] public IReadOnlyList<Recipe> Elements { get; init; } = new List<Recipe>().AsReadOnly();
}
