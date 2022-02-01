namespace SilentMike.DietMenu.Core.Application.IngredientTypes.Queries;

using System.Text.Json.Serialization;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;

public sealed record GetIngredientTypesGrid : IRequest<IngredientTypesGrid>, IAuthRequest
{
    [JsonPropertyName("grid_request")] public GridRequest GridRequest { get; init; } = new();
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
