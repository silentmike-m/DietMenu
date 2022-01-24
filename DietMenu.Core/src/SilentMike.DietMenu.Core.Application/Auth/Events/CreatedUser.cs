namespace SilentMike.DietMenu.Core.Application.Auth.Events;

using MediatR;

public sealed record CreatedUser : INotification
{
    public string Email { get; init; } = string.Empty;
    public Guid FamilyId { get; init; } = Guid.Empty;
    public string FamilyName { get; init; } = string.Empty;
    public Guid Id { get; init; } = Guid.Empty;
    public string UserName { get; init; } = string.Empty;
}
