namespace SilentMike.DietMenu.Auth.Application.Users.ViewModels;

public sealed record UserStatus
{
    public bool IsEmailConfirmed { get; init; } = default;
    public bool IsLockedOut { get; init; } = default;
}
