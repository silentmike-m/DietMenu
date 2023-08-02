namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;

using System.ComponentModel.DataAnnotations;

public sealed class LoginInputModel
{
    [EmailAddress, Required(ErrorMessage = "Należy podać email")]
    public string Email { get; set; } = string.Empty;

    [DataType(DataType.Password), Display(Name = "Hasło"), Required(ErrorMessage = "Należy podać hasło")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; } = default;

    public string ReturnUrl { get; set; } = string.Empty;
}
