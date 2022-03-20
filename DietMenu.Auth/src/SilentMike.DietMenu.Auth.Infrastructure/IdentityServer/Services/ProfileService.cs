namespace SilentMike.DietMenu.Auth.Infrastructure.IdentityServer.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class ProfileService : IProfileService
{
    private readonly UserManager<DietMenuUser> userManager;

    public ProfileService(UserManager<DietMenuUser> userManager) => this.userManager = userManager;

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await this.userManager.GetUserAsync(context.Subject);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.Id),

            new(DietMenuClaimNames.FamilyId, user.FamilyId.ToString()),
            new(DietMenuClaimNames.UserId, user.Id),
        };

        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await this.userManager.GetUserAsync(context.Subject);
        context.IsActive = user is not null;
    }
}
