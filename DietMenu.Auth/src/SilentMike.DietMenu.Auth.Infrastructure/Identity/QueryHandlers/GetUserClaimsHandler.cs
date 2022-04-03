namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.QueryHandlers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Models;
using SilentMike.DietMenu.Auth.Application.Users.Queries;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class GetUserClaimsHandler : IRequestHandler<GetUserClaims, UserClaims>
{
    private readonly ILogger<GetUserClaimsHandler> logger;
    private readonly UserManager<DietMenuUser> userManager;

    public GetUserClaimsHandler(ILogger<GetUserClaimsHandler> logger, UserManager<DietMenuUser> userManager)
        => (this.logger, this.userManager) = (logger, userManager);

    public async Task<UserClaims> Handle(GetUserClaims request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email)
        );
        this.logger.LogInformation("Try to get user claims");

        var dietMenuUser = await this.userManager.FindByEmailAsync(request.Email);

        if (dietMenuUser is null)
        {
            throw new UserNotFoundException(request.Email);
        }

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, dietMenuUser.Email),
            new(JwtRegisteredClaimNames.Sub, dietMenuUser.Id),
            new(DietMenuClaimNames.FamilyId, dietMenuUser.FamilyId.ToString()),
            new(DietMenuClaimNames.UserId, dietMenuUser.Id),
        };

        var result = new UserClaims
        {
            Claims = claims.AsReadOnly(),
            UserId = dietMenuUser.Id,
        };

        return result;
    }
}
