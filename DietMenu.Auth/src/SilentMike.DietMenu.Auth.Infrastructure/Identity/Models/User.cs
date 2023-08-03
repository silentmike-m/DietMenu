namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

using Microsoft.AspNetCore.Identity;
using SilentMike.DietMenu.Auth.Domain.Enums;

internal sealed class User : IdentityUser
{
    private Family family = null!;

    public Family Family
    {
        get => this.family;
        set
        {
            this.family = value;
            this.FamilyId = value.Id;
            this.FamilyKey = value.Key;
        }
    }

    public Guid FamilyId { get; set; } = Guid.Empty;
    public int FamilyKey { get; set; } = default;
    public string FirstName { get; set; } = string.Empty;
    public DateTime? LastLogin { get; set; } = default;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
}
