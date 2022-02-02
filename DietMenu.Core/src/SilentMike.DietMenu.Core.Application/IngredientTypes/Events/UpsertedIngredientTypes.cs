namespace SilentMike.DietMenu.Core.Application.IngredientTypes.Events;

using System.Text.Json.Serialization;

public sealed record UpsertedIngredientTypes : INotification
{
    [JsonPropertyName("ids")] public IReadOnlyList<Guid> Ids { get; init; } = new List<Guid>().AsReadOnly();
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
