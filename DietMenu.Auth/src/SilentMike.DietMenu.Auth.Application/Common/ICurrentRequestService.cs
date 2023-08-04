namespace SilentMike.DietMenu.Auth.Application.Common;

public interface ICurrentRequestService
{
    (Guid? familyId, Guid userId) CurrentUser { get; }
    string CurrentUserRole { get; }
}
