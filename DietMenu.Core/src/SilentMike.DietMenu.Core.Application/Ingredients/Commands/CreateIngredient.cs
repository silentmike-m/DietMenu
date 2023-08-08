namespace SilentMike.DietMenu.Core.Application.Ingredients.Commands;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Common.Models;
using SilentMike.DietMenu.Core.Application.Ingredients.ValueModels;

public sealed record CreateIngredient : IRequest, IAuthRequest, IFamilyRequest
{
    [JsonIgnore] public IAuthData AuthData { get; set; } = new AuthData();
    [JsonIgnore] public Guid FamilyId => this.AuthData.FamilyId;
    [JsonPropertyName("ingredient")] public IngredientToCreate Ingredient { get; init; } = new();
}
