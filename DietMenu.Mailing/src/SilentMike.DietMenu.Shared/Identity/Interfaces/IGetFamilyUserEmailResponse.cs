namespace SilentMike.DietMenu.Shared.Identity.Interfaces;

public interface IGetFamilyUserEmailResponse
{
    string Email { get; }
    Guid FamilyId { get; }
    Guid UserId { get; }
}
