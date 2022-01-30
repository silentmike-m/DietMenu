namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

[ExcludeFromCodeCoverage]
public sealed class DietMenuUser : IdentityUser
{
    [Required] public DietMenuFamily Family { get; set; } = null!;
    [ForeignKey("FamilyId")] public Guid FamilyId { get; set; } = Guid.Empty;
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public bool IsSystem { get; set; } = false;
    public DateTime? LastLogin { get; set; } = default;
    public string LastName { get; set; } = string.Empty;
}
