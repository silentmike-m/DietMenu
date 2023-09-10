namespace SilentMike.DietMenu.Core.Application.Families.Queries;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Common.Models;
using SilentMike.DietMenu.Core.Application.Families.Models;

public sealed record GetIngredientTypes : IRequest<IReadOnlyList<IngredientType>>, IAuthRequest, IFamilyRequest
{
    public IAuthData AuthData { get; init; } = new AuthData();
    public Guid FamilyId => this.AuthData.FamilyId;
}
