namespace SilentMike.DietMenu.Core.Infrastructure.MailingServer.Models.Core;

using System.Text.Json.Serialization;

internal sealed record ImportedCoreDataError
{
    [JsonPropertyName("code")] public string Code { get; init; } = string.Empty;
    [JsonPropertyName("messages")] public List<string> Messages { get; init; } = new();
}
