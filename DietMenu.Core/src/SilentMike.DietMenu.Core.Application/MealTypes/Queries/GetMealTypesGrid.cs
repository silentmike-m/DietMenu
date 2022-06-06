namespace SilentMike.DietMenu.Core.Application.MealTypes.Queries;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels;

public sealed record GetMealTypesGrid : IRequest<MealTypesGrid>, IAuthRequest
{
    [JsonPropertyName("grid_request")] public GridRequest GridRequest { get; init; } = new();
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
