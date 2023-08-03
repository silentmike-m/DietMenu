namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;

using System.ComponentModel.DataAnnotations;

public sealed class ResetPasswordInputModel
{
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match."), DataType(DataType.Password), Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [EmailAddress, Required]
    public string Email { get; set; } = string.Empty;

    [DataType(DataType.Password), Required, StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}
