namespace SilentMike.DietMenu.Core.WebApi.Services;

using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Infrastructure.IdentityServer4.Models;

[ExcludeFromCodeCoverage]
internal class AuthService : IAuthService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public (Guid? familyId, Guid userId) CurrentUser
    {
        get
        {
            var familyIdentifier = this.httpContextAccessor.HttpContext?.User.FindFirstValue(DietMenuClaimNames.FAMILY_ID);
            var userIdentifier = this.httpContextAccessor.HttpContext?.User.FindFirstValue(DietMenuClaimNames.USER_ID);

            var familyId = string.IsNullOrEmpty(familyIdentifier)
                ? (Guid?)null
                : new Guid(familyIdentifier);

            var userId = string.IsNullOrEmpty(userIdentifier)
                ? Guid.Empty
                : new Guid(userIdentifier);

            return (familyId, userId);
        }
    }

    public AuthService(IHttpContextAccessor httpContextAccessor)
        => this.httpContextAccessor = httpContextAccessor;
}
