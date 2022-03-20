namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Models;

using SilentMike.DietMenu.Shared.MassTransit.Identity;

internal sealed record GetFamilyUserEmailRequest : IGetFamilyUserEmailRequest
{
    public Guid FamilyId { get; init; } = Guid.Empty;
}
