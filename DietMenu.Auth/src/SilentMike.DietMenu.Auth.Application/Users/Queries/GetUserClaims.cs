namespace SilentMike.DietMenu.Auth.Application.Users.Queries;

using SilentMike.DietMenu.Auth.Application.Users.Models;

public sealed record GetUserClaims : IRequest<UserClaims>
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
}
