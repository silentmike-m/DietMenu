namespace SilentMike.DietMenu.Mailing.Application.Family.Commands;

using System.Text.Json.Serialization;
using SilentMike.DietMenu.Mailing.Application.Family.ValueModels;

public sealed record SendImportedFamilyDataEmail : IRequest
{
    [JsonPropertyName("data_errors")] public List<ImportedFamilyDataAreaErrors> DataErrors { get; init; } = new();
    [JsonPropertyName("family_id")] public Guid FamilyId { get; init; } = Guid.Empty;
    [JsonPropertyName("is_success")] public bool IsSuccess { get; init; } = default;
}
