namespace DietMenu.Core.Application.Users.Events;

using MediatR;

public sealed record CreatedUser : INotification
{
    public Guid FamilyId { get; init; } = Guid.Empty;
    public string FamilyName { get; init; } = string.Empty;
    public Guid UserId { get; init; } = Guid.Empty;
}
