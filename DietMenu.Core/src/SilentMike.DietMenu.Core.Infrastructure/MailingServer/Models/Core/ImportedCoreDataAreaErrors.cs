namespace SilentMike.DietMenu.Core.Infrastructure.MailingServer.Models.Core;

using System.Text.Json.Serialization;

internal sealed record ImportedCoreDataAreaErrors
{
    [JsonPropertyName("data_area")] public string DataArea { get; init; } = string.Empty;
    [JsonPropertyName("errors")] public List<ImportedCoreDataError> Errors { get; init; } = new();
}
