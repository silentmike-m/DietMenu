namespace SilentMike.DietMenu.Core.Infrastructure.IdentityServer4;

internal sealed class IdentityServer4Options
{
    public static readonly string SectionName = "IdentityServer4";
    public string Audience { get; set; } = string.Empty;
    public string Authority { get; set; } = string.Empty;
}
