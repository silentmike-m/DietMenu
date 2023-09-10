namespace SilentMike.DietMenu.Proxy.Infrastructure.IdentityServer4;

public sealed record IdentityServer4Options
{
    public static readonly string SECTION_NAME = "IdentityServer4";

    public string Audience { get; set; } = string.Empty;
    public string Authority { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}
