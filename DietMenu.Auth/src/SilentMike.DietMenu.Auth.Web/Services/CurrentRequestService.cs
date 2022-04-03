namespace SilentMike.DietMenu.Auth.Web.Services;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class CurrentRequestService : ICurrentRequestService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CurrentRequestService(IHttpContextAccessor httpContextAccessor) =>
        this.httpContextAccessor = httpContextAccessor;

    public (Guid familyId, Guid userId) CurrentUser
    {
        get
        {
            var userIdentifier = this.httpContextAccessor.HttpContext?.User.FindFirstValue(DietMenuClaimNames.UserId);
            var familyIdentifier = this.httpContextAccessor.HttpContext?.User.FindFirstValue(DietMenuClaimNames.FamilyId);

            var userId = string.IsNullOrEmpty(userIdentifier)
                ? Guid.Empty
                : new Guid(userIdentifier);

            var familyId = string.IsNullOrEmpty(familyIdentifier)
                ? Guid.Empty
                : new Guid(familyIdentifier);

            return (familyId, userId);
        }
    }
    public string Schema => this.httpContextAccessor!.HttpContext!.Request.Scheme;
}
