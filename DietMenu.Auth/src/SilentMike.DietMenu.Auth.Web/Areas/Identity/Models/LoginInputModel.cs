namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;

using System.ComponentModel.DataAnnotations;

public sealed class LoginInputModel
{
    [Required(ErrorMessage = "Należy podać email.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;


    [Required(ErrorMessage = "Należy podać hasło.")]
    [DataType(DataType.Password)]
    [Display(Name = "Hasło")]
    public string Password { get; set; } = string.Empty;
}
