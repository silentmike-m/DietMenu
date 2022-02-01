namespace SilentMike.DietMenu.Core.Application.MealTypes.Events;

using System.Text.Json.Serialization;
using SilentMike.DietMenu.Core.Application.Common;

public sealed record UpsertedMealTypes : INotification, IAuthRequest
{
    [JsonPropertyName("ids")] public IReadOnlyList<Guid> Ids { get; init; } = new List<Guid>().AsReadOnly();
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
