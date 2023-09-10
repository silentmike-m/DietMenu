namespace SilentMike.DietMenu.Core.WebApi.ViewModels.Family;

public sealed record MealType
{
    [JsonPropertyName("name")] public string Name { get; init; } = string.Empty;
    [JsonPropertyName("type")] public string Type { get; init; } = string.Empty;

    public MealType(string name, string type)
    {
        this.Name = name;
        this.Type = type;
    }
}
