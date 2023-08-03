namespace SilentMike.DietMenu.Auth.Application.Users.Commands;

public sealed record ResetUserPassword : IRequest
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
}
