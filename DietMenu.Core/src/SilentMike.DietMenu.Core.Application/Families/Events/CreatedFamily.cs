namespace SilentMike.DietMenu.Core.Application.Families.Events;

public sealed record CreatedFamily : INotification
{
    public Guid Id { get; init; } = Guid.Empty;
}
