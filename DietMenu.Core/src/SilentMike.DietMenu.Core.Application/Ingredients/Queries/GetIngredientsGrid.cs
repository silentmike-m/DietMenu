namespace SilentMike.DietMenu.Core.Application.Ingredients.Queries;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;

public sealed record GetIngredientsGrid : IRequest<IngredientsGrid>, IAuthRequest
{
    [JsonPropertyName("grid_request")] public GridRequest GridRequest { get; init; } = new();
    [JsonPropertyName("type_id")] public Guid? TypeId { get; init; } = default;
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
