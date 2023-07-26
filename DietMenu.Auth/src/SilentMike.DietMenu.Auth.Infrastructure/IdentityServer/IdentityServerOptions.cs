namespace SilentMike.DietMenu.Auth.Infrastructure.IdentityServer;

internal sealed class IdentityServerOptions
{
    public static readonly string SECTION_NAME = "IdentityServer";

    public string IssuerUri { get; set; } = string.Empty;
}
