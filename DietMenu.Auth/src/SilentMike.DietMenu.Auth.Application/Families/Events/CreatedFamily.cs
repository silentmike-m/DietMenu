namespace SilentMike.DietMenu.Auth.Application.Families.Events;

public sealed record CreatedFamily : INotification
{
    public Guid Id { get; init; } = Guid.Empty;
}
