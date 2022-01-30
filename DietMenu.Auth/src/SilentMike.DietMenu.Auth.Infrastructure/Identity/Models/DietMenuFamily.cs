namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public sealed class DietMenuFamily
{
    [Key] public Guid Id { get; set; } = Guid.Empty;
    [Required] public string Name { get; set; } = string.Empty;
}
