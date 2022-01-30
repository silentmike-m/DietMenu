namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;

using System.ComponentModel.DataAnnotations;

public sealed class RegisterInputModel
{
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Hasło i potwierdzenie hasło nie są zgodne.")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Należy podać email.")]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Należy podać nazwę rodziny.")]
    [Display(Name = "Rodzina")]
    public string Family { get; set; } = string.Empty;

    [Required(ErrorMessage = "Należy podać imię.")]
    [Display(Name = "Imię")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Imię")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Należy podać hasło.")]
    [StringLength(100, ErrorMessage = "{0} musi mieć conajmniej {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Hasło")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Należy podać kod autoryzacyjny.")]
    [Display(Name = "Kod autoryzacyjny")]
    public string RegisterCode { get; set; } = string.Empty;
}
