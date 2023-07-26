namespace SilentMike.DietMenu.Auth.Application.Users.Commands;

public sealed class GenerateEmailConfirmationToken : IRequest
{
    public Guid Id { get; init; } = Guid.Empty;
}
