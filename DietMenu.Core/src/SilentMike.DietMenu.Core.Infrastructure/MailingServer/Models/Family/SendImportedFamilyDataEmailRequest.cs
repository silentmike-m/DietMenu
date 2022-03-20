namespace SilentMike.DietMenu.Core.Infrastructure.MailingServer.Models.Family;

using System.Text.Json.Serialization;

internal sealed record SendImportedFamilyDataEmailRequest
{
    [JsonPropertyName("data_errors")] public List<ImportedFamilyDataAreaErrors> DataErrors { get; init; } = new();
    [JsonPropertyName("family_id")] public Guid FamilyId { get; init; } = Guid.Empty;
    [JsonPropertyName("is_success")] public bool IsSuccess { get; init; } = default;
    [JsonPropertyName("server")] public string Server { get; init; } = string.Empty;
}
