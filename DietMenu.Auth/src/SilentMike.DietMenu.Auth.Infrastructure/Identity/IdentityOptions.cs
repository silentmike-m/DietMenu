namespace SilentMike.DietMenu.Auth.Infrastructure.Identity;

public sealed class IdentityOptions
{
    public static readonly string SectionName = "Identity";
    public string RegisterCode { get; set; } = string.Empty;
}
