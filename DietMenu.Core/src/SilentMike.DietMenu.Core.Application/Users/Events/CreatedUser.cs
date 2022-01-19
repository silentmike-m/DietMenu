namespace SilentMike.DietMenu.Core.Application.Users.Events;

using MediatR;

public sealed record CreatedUser : INotification
{
    public string Email { get; init; } = string.Empty;
    public Guid FamilyId { get; init; } = Guid.Empty;
    public string FamilyName { get; init; } = string.Empty;
    public Guid Id { get; init; } = Guid.Empty;
    public string LoginUrl { get; set; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
}
