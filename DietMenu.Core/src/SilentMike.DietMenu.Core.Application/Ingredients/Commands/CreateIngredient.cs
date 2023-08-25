namespace SilentMike.DietMenu.Core.Application.Ingredients.Commands;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Common.Models;
using SilentMike.DietMenu.Core.Application.Ingredients.Models;

public sealed record CreateIngredient : IRequest, IAuthRequest, IFamilyRequest
{
    public IAuthData AuthData { get; set; } = new AuthData();
    public Guid FamilyId => this.AuthData.FamilyId;
    public IngredientToCreate Ingredient { get; init; } = new();
}
