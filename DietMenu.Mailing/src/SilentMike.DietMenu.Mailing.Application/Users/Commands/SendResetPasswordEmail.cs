namespace SilentMike.DietMenu.Mailing.Application.Users.Commands;

using System.Text.Json.Serialization;

public sealed record SendResetPasswordEmail : IRequest
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
    [JsonPropertyName("url")] public string Url { get; init; } = string.Empty;
}
