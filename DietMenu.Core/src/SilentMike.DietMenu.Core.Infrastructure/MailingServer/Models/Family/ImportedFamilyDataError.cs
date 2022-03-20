namespace SilentMike.DietMenu.Core.Infrastructure.MailingServer.Models.Family;

using System.Text.Json.Serialization;

internal sealed record ImportedFamilyDataError
{
    [JsonPropertyName("code")] public string Code { get; init; } = string.Empty;
    [JsonPropertyName("messages")] public List<string> Messages { get; init; } = new();
}
