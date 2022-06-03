namespace SilentMike.DietMenu.Shared.Identity.Models;

public sealed record GetFamilyUserEmailPayload
{
    public Guid FamilyId { get; init; } = Guid.Empty;
}
