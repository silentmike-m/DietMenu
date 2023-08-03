namespace SilentMike.DietMenu.Auth.Application.Users.Events;

public sealed record CreatedUser : INotification
{
    public string Email { get; init; } = string.Empty;
    public Guid Id { get; init; } = Guid.Empty;
}
