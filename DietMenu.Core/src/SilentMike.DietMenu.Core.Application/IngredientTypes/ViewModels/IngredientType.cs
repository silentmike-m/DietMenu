namespace SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;

using System.Text.Json.Serialization;

public sealed record IngredientType
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonPropertyName("name")] public string Name { get; init; } = string.Empty;
}
