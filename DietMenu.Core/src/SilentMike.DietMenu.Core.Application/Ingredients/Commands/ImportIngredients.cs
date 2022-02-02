namespace SilentMike.DietMenu.Core.Application.Ingredients.Commands;

using System.Text.Json.Serialization;

public sealed record ImportIngredients : IRequest
{
    [JsonPropertyName("family_id")] public Guid FamilyId { get; init; } = Guid.Empty;
    [JsonPropertyName("payload")] public byte[] Payload { get; init; } = Array.Empty<byte>();
}
