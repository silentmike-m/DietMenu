namespace SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;

public sealed record IngredientType
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonPropertyName("name")] public string Name { get; init; } = string.Empty;
}
