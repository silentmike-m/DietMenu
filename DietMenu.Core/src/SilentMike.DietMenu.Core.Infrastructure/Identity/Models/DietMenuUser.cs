namespace SilentMike.DietMenu.Core.Infrastructure.Identity.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public sealed class DietMenuUser : IdentityUser<Guid>
{
    [Required] public Guid FamilyId { get; set; } = Guid.Empty;
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public bool IsSystem { get; set; } = false;
    public DateTime? LastLogin { get; set; } = default;
    public string LastName { get; set; } = string.Empty;
}
