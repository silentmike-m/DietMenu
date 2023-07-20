namespace SilentMike.DietMenu.Auth.Infrastructure.Identity;

public sealed class IdentityOptions
{
    public static readonly string SECTION_NAME = "Identity";

    public string DefaultClientHost { get; set; } = "localhost";
    public bool RequireConfirmedAccount { get; set; } = default;
    public string SystemUserEmail { get; set; } = string.Empty;
    public string SystemUserPassword { get; set; } = string.Empty;
}
