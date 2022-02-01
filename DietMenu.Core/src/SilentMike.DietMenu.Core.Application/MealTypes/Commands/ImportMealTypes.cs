namespace SilentMike.DietMenu.Core.Application.MealTypes.Commands;

using System.Text.Json.Serialization;

public sealed record ImportMealTypes : IRequest
{
    [JsonPropertyName("family_id")] public Guid FamilyId { get; set; } = Guid.Empty;
}
