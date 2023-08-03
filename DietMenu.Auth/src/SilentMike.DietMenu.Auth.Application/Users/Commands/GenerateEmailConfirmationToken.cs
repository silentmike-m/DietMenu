namespace SilentMike.DietMenu.Auth.Application.Users.Commands;

public sealed class GenerateEmailConfirmationToken : IRequest
{
    public string Email { get; init; } = string.Empty;
}
