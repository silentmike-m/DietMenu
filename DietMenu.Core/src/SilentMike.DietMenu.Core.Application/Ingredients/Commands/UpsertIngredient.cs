namespace SilentMike.DietMenu.Core.Application.Ingredients.Commands;

using System.Text.Json.Serialization;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Ingredients.ValueModels;

public sealed record UpsertIngredient : IRequest, IAuthRequest
{
    [JsonPropertyName("ingredient")] public IngredientToUpsert Ingredient { get; init; } = new();
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
