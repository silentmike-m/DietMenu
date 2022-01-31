namespace SilentMike.DietMenu.Core.WebApi.Services;

using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Infrastructure.IdentityServer4.Models;

[ExcludeFromCodeCoverage]
internal class CurrentRequestService : ICurrentRequestService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CurrentRequestService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public (Guid familyId, Guid userId) CurrentUser
    {
        get
        {
            var familyIdentifier = this.httpContextAccessor.HttpContext?.User.FindFirstValue(DietMenuClaimNames.FamilyId);
            var userIdentifier = this.httpContextAccessor.HttpContext?.User.FindFirstValue(DietMenuClaimNames.UserId);

            var familyId = string.IsNullOrEmpty(familyIdentifier)
                ? Guid.Empty
                : new Guid(familyIdentifier);

            var userId = string.IsNullOrEmpty(userIdentifier)
                ? Guid.Empty
                : new Guid(userIdentifier);

            return (familyId, userId);
        }
    }
}
