namespace SilentMike.DietMenu.Shared.MassTransit.Identity;

public interface IGetFamilyUserEmailRequest
{
    Guid FamilyId { get; }
}
