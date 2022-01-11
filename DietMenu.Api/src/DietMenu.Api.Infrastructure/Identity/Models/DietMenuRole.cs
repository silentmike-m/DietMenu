namespace DietMenu.Api.Infrastructure.Identity.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public sealed class DietMenuRole : IdentityRole<Guid>
{
    [Required] public bool IsSystem { get; set; } = false;
}
