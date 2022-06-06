namespace SilentMike.DietMenu.Core.Application.Common;

public interface IAuthService
{
    (Guid familyId, Guid userId) CurrentUser { get; }
}
