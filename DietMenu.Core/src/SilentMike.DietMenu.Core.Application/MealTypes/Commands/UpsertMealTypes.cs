namespace SilentMike.DietMenu.Core.Application.MealTypes.Commands;
using System.Text.Json.Serialization;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels.ValueModels;

public sealed record UpsertMealTypes : IRequest, IAuthRequest
{
    [JsonPropertyName("meal_types")] public IReadOnlyList<MealTypeToUpsert> MealTypes { get; set; } = new List<MealTypeToUpsert>().AsReadOnly();
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
