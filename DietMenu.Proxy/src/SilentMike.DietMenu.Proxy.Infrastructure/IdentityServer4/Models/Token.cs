namespace SilentMike.DietMenu.Proxy.Infrastructure.IdentityServer4.Models;

internal sealed record Token
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; } = DateTime.MinValue;
    public int ExpiresInSeconds { get; set; } = default;
    public bool IsRefreshing { get; set; } = default;
    public string RefreshToken { get; init; } = string.Empty;
}
