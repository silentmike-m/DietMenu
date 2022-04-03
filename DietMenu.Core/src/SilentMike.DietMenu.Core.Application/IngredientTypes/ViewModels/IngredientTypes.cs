namespace SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;

using System.Text.Json.Serialization;

public sealed record IngredientTypes
{
    [JsonPropertyName("types")] public IReadOnlyList<IngredientType> Types { get; init; } = new List<IngredientType>().AsReadOnly();
}
