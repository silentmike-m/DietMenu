namespace SilentMike.DietMenu.Auth.Application.Users.Models;

public sealed record UserActivationStatus
{
    public bool IsActive { get; init; } = default;
}
