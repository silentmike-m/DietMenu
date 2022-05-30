namespace SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;

using System.Text.Json.Serialization;

public sealed record ReplacementsGrid
{
    [JsonPropertyName("count")] public int Count { get; init; } = default;
    [JsonPropertyName("elements")] public IReadOnlyList<Replacement> Elements { get; init; } = new List<Replacement>().AsReadOnly();
}
