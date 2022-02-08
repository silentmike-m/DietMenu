namespace SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;

using System.Text.Json.Serialization;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels.ValueModels;

public sealed record UpsertIngredientType : IRequest, IAuthRequest
{
    [JsonPropertyName("ingredient_type")] public IngredientTypeToUpsert IngredientType { get; init; } = new();
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
