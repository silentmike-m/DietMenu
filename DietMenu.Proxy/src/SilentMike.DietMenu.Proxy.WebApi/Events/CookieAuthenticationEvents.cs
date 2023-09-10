namespace SilentMike.DietMenu.Proxy.WebApi.Events;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SilentMike.DietMenu.Proxy.Infrastructure.IdentityServer4.Interfaces;

internal sealed class CookieAuthenticationEvents : Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
{
    private readonly IIdentityServerService identityServerService;

    public CookieAuthenticationEvents(IIdentityServerService identityServerService)
        => this.identityServerService = identityServerService;

    public override Task SignedIn(CookieSignedInContext context)
    {
        base.SignedIn(context);

        var accessToken = context.Properties.GetTokenValue(OpenIdConnectParameterNames.AccessToken);
        var expiresAt = context.Properties.GetTokenValue("expires_at");
        var refreshToken = context.Properties.GetTokenValue(OpenIdConnectParameterNames.RefreshToken);
        var sessionId = context.HttpContext.Session.Id;

        var canSetToken = !string.IsNullOrEmpty(accessToken)
                          && !string.IsNullOrEmpty(expiresAt)
                          && !string.IsNullOrEmpty(refreshToken)
                          && !string.IsNullOrEmpty(sessionId);

        if (canSetToken)
        {
            this.identityServerService.CreateToken(accessToken!, expiresAt!, refreshToken!, sessionId);

            context.HttpContext.Session.SetString("sessionId", context.HttpContext.Session.Id);

            context.HttpContext.Session.CommitAsync()
                .GetAwaiter()
                .GetResult();
        }

        return Task.CompletedTask;
    }
}
