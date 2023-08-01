namespace SilentMike.DietMenu.Auth.Application.Users.Queries;

using SilentMike.DietMenu.Auth.Application.Users.ViewModels;

public sealed record GetUserStatus : IRequest<UserStatus>
{
    public string Email { get; init; } = string.Empty;
}
