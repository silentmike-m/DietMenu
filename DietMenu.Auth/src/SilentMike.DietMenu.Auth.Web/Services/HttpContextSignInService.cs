namespace SilentMike.DietMenu.Auth.Web.Services;

using System.Security.Claims;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using SilentMike.DietMenu.Auth.Application.Auth.Events;
using SilentMike.DietMenu.Auth.Application.Auth.Queries;
using SilentMike.DietMenu.Auth.Web.Interfaces;

internal sealed class HttpContextSignInService : IHttpContextSignInService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<HttpContextSignInService> logger;
    private readonly IMediator mediator;

    public HttpContextSignInService(IHttpContextAccessor httpContextAccessor, ILogger<HttpContextSignInService> logger, IMediator mediator)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task SignInAsync(string email, CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Try to sign in user in http context");

        var getUserClaimsRequest = new GetUserClaims
        {
            Email = email,
        };

        var userClaims = await this.mediator.Send(getUserClaimsRequest, CancellationToken.None);

        var additionalClaims = new List<Claim>();

        foreach (var userClaim in userClaims.Claims)
        {
            var claim = new Claim(userClaim.Key, userClaim.Value);

            additionalClaims.Add(claim);
        }

        var userId = userClaims.UserId.ToString();

        var issuer = new IdentityServerUser(userId)
        {
            DisplayName = email,
            AdditionalClaims = additionalClaims,
        };

        AuthenticationProperties? authenticationProperties = null;

        await this.httpContextAccessor.HttpContext.SignInAsync(issuer, authenticationProperties);

        var notification = new UserLoggedIn
        {
            UserId = userClaims.UserId,
        };

        await this.mediator.Publish(notification, CancellationToken.None);
    }
}
