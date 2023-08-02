namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;

using SilentMike.DietMenu.Shared.Identity.Interfaces;

internal sealed record GetFamilyOwnerEmailResponse : IGetFamilyOwnerEmailResponse
{
    public string Email { get; init; } = string.Empty;
    public Guid FamilyId { get; init; } = Guid.Empty;
    public Guid UserId { get; init; } = Guid.Empty;
}
