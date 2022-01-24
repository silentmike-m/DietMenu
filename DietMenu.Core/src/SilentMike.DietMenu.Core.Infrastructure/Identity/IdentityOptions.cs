namespace SilentMike.DietMenu.Core.Infrastructure.Identity;

public sealed class IdentityOptions
{
    public static readonly string SectionName = "Identity";
    public string CreateUserCode { get; set; } = string.Empty;
}
