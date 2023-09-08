namespace SilentMike.DietMenu.Shared.Identity.Interfaces;

public interface IGetFamilyEmailResponse
{
    string Email { get; }
    Guid FamilyId { get; }
}
