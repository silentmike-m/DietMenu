namespace SilentMike.DietMenu.Shared.Identity.Interfaces;

public interface IGetFamilyOwnerEmailResponse
{
    string Email { get; }
    Guid FamilyId { get; }
    Guid UserId { get; }
}
