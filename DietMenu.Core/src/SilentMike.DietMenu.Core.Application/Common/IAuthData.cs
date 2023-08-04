namespace SilentMike.DietMenu.Core.Application.Common;

public interface IAuthData
{
    Guid FamilyId { get; set; }
    Guid UserId { get; set; }
}
