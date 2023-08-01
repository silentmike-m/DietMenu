namespace SilentMike.DietMenu.Auth.Application.Common;

public interface IAuthData
{
    Guid FamilyId { get; set; }
    Guid UserId { get; set; }
}
