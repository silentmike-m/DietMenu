namespace SilentMike.DietMenu.Auth.Application.Auth.Events;

public sealed class UserLoggedIn : INotification
{
    public Guid UserId { get; init; } = Guid.Empty;
}
