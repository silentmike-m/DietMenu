namespace SilentMike.DietMenu.Core.Application.Common;

public interface ICurrentRequestService
{
    (Guid familyId, Guid userId) CurrentUser { get; }
}
