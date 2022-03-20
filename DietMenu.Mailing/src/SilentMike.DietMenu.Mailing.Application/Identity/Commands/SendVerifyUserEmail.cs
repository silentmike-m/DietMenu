namespace SilentMike.DietMenu.Mailing.Application.Identity.Commands;

using System.Text.Json.Serialization;

public sealed record SendVerifyUserEmail : IRequest
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
    [JsonPropertyName("url")] public string Url { get; init; } = string.Empty;
}
