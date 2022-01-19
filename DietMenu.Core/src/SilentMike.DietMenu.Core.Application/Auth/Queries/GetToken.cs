namespace SilentMike.DietMenu.Core.Application.Auth.Queries;

using System.Text.Json.Serialization;

public sealed record GetToken : IRequest<string>
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
    [JsonPropertyName("password")] public string Password { get; init; } = string.Empty;
}
