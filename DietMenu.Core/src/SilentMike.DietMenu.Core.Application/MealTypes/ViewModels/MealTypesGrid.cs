namespace SilentMike.DietMenu.Core.Application.MealTypes.ViewModels;

using System.Text.Json.Serialization;

public sealed record MealTypesGrid
{
    [JsonPropertyName("count")] public int Count { get; init; } = default;
    [JsonPropertyName("elements")] public IReadOnlyList<MealType> Elements { get; init; } = new List<MealType>().AsReadOnly();
}
