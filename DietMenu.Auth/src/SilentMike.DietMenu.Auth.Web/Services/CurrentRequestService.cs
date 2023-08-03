namespace SilentMike.DietMenu.Auth.Web.Services;

using System.Security.Claims;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class CurrentRequestService : ICurrentRequestService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public (Guid? familyId, Guid userId) CurrentUser
    {
        get
        {
            var userIdentifier = this.httpContextAccessor.HttpContext?.User.FindFirstValue(DietMenuClaimNames.USER_ID);
            var familyIdentifier = this.httpContextAccessor.HttpContext?.User.FindFirstValue(DietMenuClaimNames.FAMILY_ID);

            var userId = string.IsNullOrEmpty(userIdentifier)
                ? Guid.Empty
                : new Guid(userIdentifier);

            var familyId = string.IsNullOrEmpty(familyIdentifier)
                ? (Guid?)null
                : new Guid(familyIdentifier);

            return (familyId, userId);
        }
    }

    public string CurrentUserRole
    {
        get
        {
            var role = this.httpContextAccessor.HttpContext?.User.FindFirstValue(DietMenuClaimNames.ROLE);
            role ??= string.Empty;

            return role;
        }
    }

    public string Schema => this.httpContextAccessor!.HttpContext!.Request.Scheme;

    public CurrentRequestService(IHttpContextAccessor httpContextAccessor) =>
        this.httpContextAccessor = httpContextAccessor;
}
