namespace SilentMike.DietMenu.Core.Application.MealTypes.Commands;
using System.Text.Json.Serialization;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels.ValueModels;

public sealed record UpsertMealType : IRequest, IAuthRequest
{
    [JsonPropertyName("meal_type")] public MealTypeToUpsert MealType { get; init; } = new();
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
