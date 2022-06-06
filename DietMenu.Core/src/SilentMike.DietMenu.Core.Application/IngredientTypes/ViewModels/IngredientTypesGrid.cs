namespace SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;

public sealed record IngredientTypesGrid
{
    [JsonPropertyName("count")] public int Count { get; init; } = default;
    [JsonPropertyName("elements")] public IReadOnlyList<IngredientType> Elements { get; init; } = new List<IngredientType>();
}
