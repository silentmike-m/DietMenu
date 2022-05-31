namespace SilentMike.DietMenu.Core.Application.MealTypes.ViewModels;

public sealed record MealType
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonPropertyName("name")] public string Name { get; init; } = string.Empty;
    [JsonPropertyName("order")] public int Order { get; init; } = 1;
}
