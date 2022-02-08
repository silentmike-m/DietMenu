namespace SilentMike.DietMenu.Core.Application.Recipes.Events;
using System.Text.Json.Serialization;
using SilentMike.DietMenu.Core.Application.Common;

public sealed record UpsertedRecipe : INotification, IAuthRequest
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
