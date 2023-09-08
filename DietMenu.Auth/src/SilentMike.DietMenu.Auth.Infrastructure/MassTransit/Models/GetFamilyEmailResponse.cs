namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;

using SilentMike.DietMenu.Shared.Identity.Interfaces;

internal sealed record GetFamilyEmailResponse : IGetFamilyEmailResponse
{
    public string Email { get; init; } = string.Empty;
    public Guid FamilyId { get; init; } = Guid.Empty;
}
