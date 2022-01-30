namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;

using System.ComponentModel.DataAnnotations;

public sealed class ForgotPasswordInputModel
{
    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
}

