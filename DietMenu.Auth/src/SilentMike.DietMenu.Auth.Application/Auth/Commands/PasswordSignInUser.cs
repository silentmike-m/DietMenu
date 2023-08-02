namespace SilentMike.DietMenu.Auth.Application.Auth.Commands;

public sealed record PasswordSignInUser : IRequest
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public bool Remember { get; init; } = default;
}
