namespace SilentMike.DietMenu.Core.Infrastructure.MailingServer.Models.Core;

using System.Text.Json.Serialization;

internal sealed record SendImportedCoreDataEmailRequest
{
    [JsonPropertyName("data_errors")] public List<ImportedCoreDataAreaErrors> DataErrors { get; init; } = new();
    [JsonPropertyName("is_success")] public bool IsSuccess { get; init; } = default;
    [JsonPropertyName("server")] public string Server { get; init; } = string.Empty;
}
