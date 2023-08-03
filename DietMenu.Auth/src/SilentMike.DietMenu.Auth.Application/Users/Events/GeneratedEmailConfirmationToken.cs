namespace SilentMike.DietMenu.Auth.Application.Users.Events;

public sealed record GeneratedEmailConfirmationToken : INotification
{
    public string Email { get; init; } = string.Empty;
    public Guid Id { get; init; } = Guid.Empty;
    public string Token { get; init; } = string.Empty;
}
