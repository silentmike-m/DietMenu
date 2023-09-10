namespace SilentMike.DietMenu.Proxy.Infrastructure.IdentityServer4.Interfaces;

public interface IIdentityServerService
{
    void ClearToken(string sessionId);
    void CreateToken(string accessToken, string expiresAt, string refreshToken, string sessionId);
    string? GetAccessToken(string sessionId);
    Task RefreshTokenAsync(string sessionId, bool force = false, CancellationToken cancellationToken = default);
}
