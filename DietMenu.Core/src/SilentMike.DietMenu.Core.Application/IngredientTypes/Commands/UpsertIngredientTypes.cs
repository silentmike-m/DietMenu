namespace SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;

using System.Text.Json.Serialization;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels.ValueModels;

public sealed record UpsertIngredientTypes : IRequest, IAuthRequest
{
    [JsonPropertyName("meal_types")]
    public IReadOnlyList<IngredientTypeToUpsert> IngredientTypes { get; set; } =
        new List<IngredientTypeToUpsert>().AsReadOnly();
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
