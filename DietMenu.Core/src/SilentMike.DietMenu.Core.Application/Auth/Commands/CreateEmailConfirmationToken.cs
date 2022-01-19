namespace SilentMike.DietMenu.Core.Application.Auth.Commands;

using System.Text.Json.Serialization;

public sealed record CreateEmailConfirmationToken : IRequest
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
}
