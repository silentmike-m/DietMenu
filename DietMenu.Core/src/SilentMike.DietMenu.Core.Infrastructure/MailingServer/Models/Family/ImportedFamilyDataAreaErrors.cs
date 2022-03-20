namespace SilentMike.DietMenu.Core.Infrastructure.MailingServer.Models.Family;

using System.Text.Json.Serialization;

internal sealed record ImportedFamilyDataAreaErrors
{
    [JsonPropertyName("data_area")] public string DataArea { get; init; } = string.Empty;
    [JsonPropertyName("errors")] public List<ImportedFamilyDataError> Errors { get; init; } = new();
}
