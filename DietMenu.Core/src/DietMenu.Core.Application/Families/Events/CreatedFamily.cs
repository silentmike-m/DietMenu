namespace DietMenu.Core.Application.Families.Events;

using MediatR;

public sealed record CreatedFamily : INotification
{
    public Guid Id { get; init; } = Guid.Empty;
}
