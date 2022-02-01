namespace SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels.ValueModels;

using System.Text.Json.Serialization;

public sealed record IngredientTypeToUpsert
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonPropertyName("name")] public string? Name { get; init; } = default;
}
