namespace SilentMike.DietMenu.Auth.Application.Users.Events;

public sealed record CreatedUser : INotification
{
    public Guid Id { get; init; } = Guid.Empty;
}
