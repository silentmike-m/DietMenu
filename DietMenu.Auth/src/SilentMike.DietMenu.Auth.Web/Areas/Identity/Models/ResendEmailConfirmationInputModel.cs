namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;

using System.ComponentModel.DataAnnotations;

public sealed class ResendEmailConfirmationInputModel
{
    [EmailAddress, Required] public string Email { get; set; } = string.Empty;
}
