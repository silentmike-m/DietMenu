namespace SilentMike.DietMenu.Core.Application.IngredientTypes.Queries;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;

public sealed record GetIngredientTypes : IRequest<IngredientTypes>, IAuthRequest
{
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
