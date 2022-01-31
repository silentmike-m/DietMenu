namespace SilentMike.DietMenu.Core.Application.Common;

public interface IAuthRequest
{
    Guid FamilyId { get; set; }
    Guid UserId { get; set; }
}
