namespace SilentMike.DietMenu.Mailing.Application.Emails.Models;

using System.Text.Json.Serialization;

public sealed record SendEmailRequest
{
    [JsonPropertyName("receiver")] public string Receiver { get; init; } = string.Empty;
    [JsonPropertyName("subject")] public string Subject { get; init; } = string.Empty;
    [JsonPropertyName("text")] public string Text { get; init; } = string.Empty;
}
