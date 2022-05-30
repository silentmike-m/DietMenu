namespace SilentMike.DietMenu.Core.Application.Ingredients.Queries;

using System.Text.Json.Serialization;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;

public sealed record GetReplacementsGrid : IRequest<ReplacementsGrid>, IAuthRequest
{
    [JsonPropertyName("exchanger")] public decimal Exchanger { get; init; } = default;
    [JsonPropertyName("grid_request")] public GridRequest GridRequest { get; init; } = new();
    [JsonPropertyName("quantity")] public decimal Quantity { get; init; } = default;
    [JsonPropertyName("type_id")] public Guid TypeId { get; init; } = default;
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
