namespace SilentMike.DietMenu.Core.Application.MealTypes.ViewModels;

public sealed record MealTypesGrid
{
    [JsonPropertyName("count")] public int Count { get; init; } = default;
    [JsonPropertyName("elements")] public IReadOnlyList<MealType> Elements { get; init; } = new List<MealType>();
}
