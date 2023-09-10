namespace SilentMike.DietMenu.Proxy.WebApi.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SilentMike.DietMenu.Proxy.Infrastructure.IdentityServer4.Interfaces;

internal sealed class TokenService
{
    private readonly IIdentityServerService identityServerService;
    private readonly ILogger<TokenService> logger;

    public TokenService(IIdentityServerService identityServerService, ILogger<TokenService> logger)
    {
        this.identityServerService = identityServerService;
        this.logger = logger;
    }

    public async Task<bool> Handle(HttpContext context)
    {
        bool result;

        try
        {
            var accessToken = this.identityServerService.GetAccessToken(context.Session.Id);

            result = await this.ValidateAccessToken(accessToken, context);

            if (result)
            {
                await this.identityServerService.RefreshTokenAsync(context.Session.Id);
            }
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            this.identityServerService.ClearToken(context.Session.Id);

            result = false;
        }

        return result;
    }

    private async Task<bool> ValidateAccessToken(string? accessToken, HttpContext context)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            this.logger.LogWarning("Missing access token for session");

            return false;
        }

        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Request.Headers.Add("Authorization", string.Empty);
        }

        context.Request.Headers["Authorization"] = $"Bearer {accessToken}";

        var result = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

        return result.Succeeded;
    }
}
