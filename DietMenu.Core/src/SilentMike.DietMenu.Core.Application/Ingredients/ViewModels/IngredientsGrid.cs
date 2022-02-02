namespace SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;

using System.Text.Json.Serialization;

public sealed record IngredientsGrid
{
    [JsonPropertyName("count")] public int Count { get; init; } = default;
    [JsonPropertyName("elements")] public IReadOnlyList<Ingredient> Elements { get; init; } = new List<Ingredient>().AsReadOnly();
}
