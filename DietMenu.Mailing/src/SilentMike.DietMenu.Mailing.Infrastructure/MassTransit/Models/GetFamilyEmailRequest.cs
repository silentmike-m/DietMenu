namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Models;

using SilentMike.DietMenu.Shared.Identity.Interfaces;

internal sealed record GetFamilyEmailRequest : IGetFamilyEmailRequest
{
    public Guid FamilyId { get; init; } = Guid.Empty;
}
