namespace SilentMike.DietMenu.Auth.Application.Users.Commands;

public sealed record GenerateResetPasswordToken : IRequest
{
    public string Email { get; init; } = string.Empty;
}
