namespace SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;

using System.Text.Json.Serialization;

public sealed class ImportIngredientTypes : IRequest
{
    [JsonPropertyName("family_id")] public Guid FamilyId { get; init; } = Guid.Empty;
}
