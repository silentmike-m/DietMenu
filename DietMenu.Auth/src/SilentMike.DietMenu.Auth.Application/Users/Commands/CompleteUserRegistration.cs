namespace SilentMike.DietMenu.Auth.Application.Users.Commands;

public sealed record CompleteUserRegistration : IRequest
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Password { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
}
