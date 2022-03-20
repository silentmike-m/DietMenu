namespace SilentMike.DietMenu.Mailing.Application.Identity.Queries;

using System.Text.Json.Serialization;

public sealed record GetFamilyUserEmail : IRequest<string>
{
    [JsonPropertyName("family_id")] public Guid FamilyId { get; init; } = Guid.Empty;
}
