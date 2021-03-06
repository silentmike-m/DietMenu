namespace SilentMike.DietMenu.Core.Application.Recipes.Commands;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Recipes.ValueModels;

public sealed record UpsertRecipe : IRequest, IAuthRequest, IFamilyRequest
{
    [JsonPropertyName("recipes")] public RecipeToUpsert Recipe { get; init; } = new();
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
