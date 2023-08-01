namespace SilentMike.DietMenu.Auth.Infrastructure.Auth.QueryHandlers;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Auth.Queries;
using SilentMike.DietMenu.Auth.Application.Auth.ViewModels;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class GetUserClaimsHandler : IRequestHandler<GetUserClaims, UserClaims>
{
    private readonly ILogger<GetUserClaimsHandler> logger;
    private readonly UserManager<User> userManager;

    public GetUserClaimsHandler(ILogger<GetUserClaimsHandler> logger, UserManager<User> userManager)
    {
        this.logger = logger;
        this.userManager = userManager;
    }

    public async Task<UserClaims> Handle(GetUserClaims request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get user claims");

        var user = await this.userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new UserNotFoundException(request.Email);
        }

        var result = new UserClaims
        {
            Claims = new Dictionary<string, string>
            {
                { JwtRegisteredClaimNames.Email, user.Email },
                { JwtRegisteredClaimNames.Sub, user.Id },
                { DietMenuClaimNames.FAMILY_ID, user.FamilyId.ToString() },
                { DietMenuClaimNames.ROLE, user.Role.ToString() },
                { DietMenuClaimNames.USER_ID, user.Id },
            },
            FamilyId = user.FamilyId,
            UserId = new Guid(user.Id),
        };

        return result;
    }
}
