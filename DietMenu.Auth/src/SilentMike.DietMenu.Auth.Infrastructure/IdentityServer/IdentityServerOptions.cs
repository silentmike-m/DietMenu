namespace SilentMike.DietMenu.Auth.Infrastructure.IdentityServer;

internal sealed class IdentityServerOptions
{
    public static readonly string SECTION_NAME = "IdentityServer";

    public string DefaultClientUri { get; set; } = string.Empty;
    public string IssuerUri { get; set; } = string.Empty;
}
