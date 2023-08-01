namespace SilentMike.DietMenu.Auth.Infrastructure.IdentityServer.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Auth.Events;
using SilentMike.DietMenu.Auth.Application.Auth.Queries;

internal sealed class ProfileService : IProfileService
{
    private readonly ILogger<ProfileService> logger;
    private readonly IMediator mediator;

    public ProfileService(ILogger<ProfileService> logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var email = GetUserEmail(context.Subject);

        var getUserClaimsRequest = new GetUserClaims
        {
            Email = email,
        };

        var userClaims = await this.mediator.Send(getUserClaimsRequest, CancellationToken.None);

        foreach (var userClaim in userClaims.Claims)
        {
            var claim = new Claim(userClaim.Key, userClaim.Value);

            context.IssuedClaims.Add(claim);
        }

        var notification = new UserLoggedIn
        {
            UserId = userClaims.UserId,
        };

        await this.mediator.Publish(notification, CancellationToken.None);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        try
        {
            // var email = GetUserEmail(context.Subject);
            //
            // var getUserActivationStatusRequest = new GetUserActivationStatus
            // {
            //     Email = email,
            // };
            //
            // var userActivationStatus = await this.mediator.Send(getUserActivationStatusRequest, CancellationToken.None);
            //
            // context.IsActive = userActivationStatus.IsActive;
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);
            context.IsActive = false;
        }
    }

    private static string GetUserEmail(ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        return principal.FindFirstValue(JwtRegisteredClaimNames.Email);
    }
}
