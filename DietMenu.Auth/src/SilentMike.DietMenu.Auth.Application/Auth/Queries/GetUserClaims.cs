namespace SilentMike.DietMenu.Auth.Application.Auth.Queries;

using SilentMike.DietMenu.Auth.Application.Auth.ViewModels;

public sealed record GetUserClaims : IRequest<UserClaims>
{
    public string Email { get; init; } = string.Empty;
}
