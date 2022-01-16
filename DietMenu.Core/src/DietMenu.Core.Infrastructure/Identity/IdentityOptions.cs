namespace DietMenu.Core.Infrastructure.Identity;

public sealed class IdentityOptions
{
    public static readonly string SECTION_NAME = "Identity";
    public string CreateUserCode { get; set; } = string.Empty;
}
