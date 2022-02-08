namespace SilentMike.DietMenu.Core.Application.Ingredients.Events;

using System.Text.Json.Serialization;

public sealed record UpsertedIngredient : INotification
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
