namespace SilentMike.DietMenu.Auth.Application.Users.Queries;

using SilentMike.DietMenu.Auth.Application.Users.Models;

public sealed record GetUserActivationStatus : IRequest<UserActivationStatus>
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
}
