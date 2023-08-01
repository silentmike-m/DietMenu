namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

using Microsoft.AspNetCore.Identity;
using SilentMike.DietMenu.Auth.Domain.Enums;

internal sealed class User : IdentityUser
{
    public Family Family { get; set; } = null!;
    public Guid FamilyId { get; set; } = Guid.Empty;
    public int FamilyKey { get; set; } = default;
    public string FirstName { get; set; } = string.Empty;
    public int Key { get; set; } = default;
    public DateTime? LastLogin { get; set; } = default;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
}
