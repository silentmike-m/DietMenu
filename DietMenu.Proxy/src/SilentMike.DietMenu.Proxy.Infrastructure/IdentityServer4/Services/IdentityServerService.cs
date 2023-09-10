namespace SilentMike.DietMenu.Proxy.Infrastructure.IdentityServer4.Services;

using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SilentMike.DietMenu.Proxy.Infrastructure.Common.Interfaces;
using SilentMike.DietMenu.Proxy.Infrastructure.IdentityServer4.Interfaces;
using SilentMike.DietMenu.Proxy.Infrastructure.IdentityServer4.Models;

internal sealed class IdentityServerService : IIdentityServerService
{
    private const string HTTP_CLIENT_NAME = "IdentityServerService";
    private const string TOKEN_ENDPOINT = "/connect/token";

    private readonly IMemoryCache cache;
    private readonly IDateTimeService dateTimeService;
    private readonly HttpClient httpClient;
    private readonly ILogger<IdentityServerService> logger;
    private readonly IdentityServer4Options options;

    public IdentityServerService(IMemoryCache cache, IDateTimeService dateTimeService, IHttpClientFactory httpClientFactory, ILogger<IdentityServerService> logger, IOptions<IdentityServer4Options> options)
    {
        this.cache = cache;
        this.dateTimeService = dateTimeService;
        this.httpClient = httpClientFactory.CreateClient(HTTP_CLIENT_NAME);
        this.logger = logger;
        this.options = options.Value;
    }

    public void ClearToken(string sessionId)
    {
        this.cache.Remove(sessionId);
    }

    public void CreateToken(string accessToken, string expiresAt, string refreshToken, string sessionId)
    {
        this.logger.LogInformation("Try to create token");

        try
        {
            var now = this.dateTimeService.GetNow();

            var expiresAtOffset = DateTimeOffset.Parse(expiresAt);
            var expiresIn = expiresAtOffset.UtcDateTime.Subtract(now);
            var expiresInSeconds = (int)expiresIn.TotalSeconds;

            this.SetToken(accessToken, expiresAtOffset.UtcDateTime, expiresInSeconds, refreshToken, sessionId);
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);
        }
    }

    public string? GetAccessToken(string sessionId)
    {
        var token = this.cache.Get<Token>(sessionId);

        return token?.AccessToken;
    }

    public async Task RefreshTokenAsync(string sessionId, bool force = false, CancellationToken cancellationToken = default)
    {
        try
        {
            var token = this.cache.Get<Token>(sessionId);

            if (token is null || token.IsRefreshing)
            {
                return;
            }

            var halfExpiresInSeconds = token.ExpiresInSeconds / 2;

            var now = this.dateTimeService.GetNow();

            var currentExpiresInSeconds = (int)token.ExpiresAt.Subtract(now).TotalSeconds;

            if (force || currentExpiresInSeconds <= halfExpiresInSeconds)
            {
                token.IsRefreshing = true;
                token.ExpiresInSeconds = currentExpiresInSeconds;

                this.SetToken(sessionId, token);

                await this.RefreshTokenAsync(sessionId, token.RefreshToken, cancellationToken);
            }
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);
        }
    }

    private async Task RefreshTokenAsync(string sessionId, string refreshToken, CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Try to refresh access token in identity server");

        var now = this.dateTimeService.GetNow();

        var request = new RefreshTokenRequest
        {
            Address = TOKEN_ENDPOINT,
            ClientId = this.options.ClientId,
            ClientSecret = this.options.ClientSecret,
            RefreshToken = refreshToken,
        };

        var response = await this.httpClient.RequestRefreshTokenAsync(request, cancellationToken);

        if (response.IsError)
        {
            throw new Exception(response.Error);
        }

        var expiresAt = now.AddSeconds(response.ExpiresIn);

        this.SetToken(response.AccessToken, expiresAt, response.ExpiresIn, response.RefreshToken, sessionId);
    }

    private void SetToken(string accessToken, DateTime expiresAt, int expiresInSeconds, string refreshToken, string sessionId)
    {
        var token = new Token
        {
            AccessToken = accessToken,
            ExpiresAt = expiresAt,
            ExpiresInSeconds = expiresInSeconds,
            RefreshToken = refreshToken,
        };

        this.SetToken(sessionId, token);
    }

    private void SetToken(string sessionId, Token token)
    {
        var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(token.ExpiresInSeconds));

        var expirationToken = new CancellationChangeToken(cancellationTokenSource.Token);

        var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
            .AddExpirationToken(expirationToken);

        this.cache.Set(sessionId, token, memoryCacheEntryOptions);
    }
}
