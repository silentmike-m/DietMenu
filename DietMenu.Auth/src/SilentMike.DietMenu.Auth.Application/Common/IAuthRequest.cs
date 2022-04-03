namespace SilentMike.DietMenu.Auth.Application.Common;

public interface IAuthRequest
{
    Guid FamilyId { get; set; }
    Guid UserId { get; set; }
}
