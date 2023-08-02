namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Pages.Account;

using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;

public class ResetPasswordModel : PageModel
{
    private readonly IMediator mediator;
    [BindProperty] public ResetPasswordInputModel Input { get; set; } = new();

    public ResetPasswordModel(IMediator mediator)
        => this.mediator = mediator;

    public IActionResult OnGet(string code = "")
    {
        if (string.IsNullOrEmpty(code))
        {
            return this.BadRequest("A code must be supplied for password reset.");
        }

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(string code = "")
    {
        if (this.ModelState.IsValid is false)
        {
            return this.Page();
        }

        try
        {
            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            // var request = new ResetPassword
            // {
            //     Email = this.Input.Email,
            //     Password = this.Input.Password,
            //     Token = token,
            // };
            //
            // await this.mediator.Send(request, CancellationToken.None);

            return this.RedirectToPage("./ResetPasswordConfirmation");
        }
        catch (Exception exception)
        {
            this.ModelState.AddModelError(string.Empty, exception.Message);

            return this.Page();
        }
    }
}
