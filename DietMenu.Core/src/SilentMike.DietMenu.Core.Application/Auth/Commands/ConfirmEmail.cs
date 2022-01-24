namespace SilentMike.DietMenu.Core.Application.Auth.Commands;

using System.Text.Json.Serialization;

public sealed record ConfirmEmail : IRequest
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
    [JsonPropertyName("token")] public string Token { get; init; } = string.Empty;
}
