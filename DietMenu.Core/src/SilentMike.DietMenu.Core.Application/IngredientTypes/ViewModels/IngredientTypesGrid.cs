namespace SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;

using System.Text.Json.Serialization;

public sealed record IngredientTypesGrid
{
    [JsonPropertyName("count")] public int Count { get; init; } = default;
    [JsonPropertyName("elements")] public IReadOnlyList<IngredientType> Elements { get; init; } = new List<IngredientType>().AsReadOnly();
}
