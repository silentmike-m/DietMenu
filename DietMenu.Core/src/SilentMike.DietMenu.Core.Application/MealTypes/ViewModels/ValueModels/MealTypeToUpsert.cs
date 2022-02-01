namespace SilentMike.DietMenu.Core.Application.MealTypes.ViewModels.ValueModels;

using System.Text.Json.Serialization;

public sealed record MealTypeToUpsert
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonPropertyName("name")] public string? Name { get; init; } = default;
    [JsonPropertyName("order")] public int? Order { get; init; } = default;
}
