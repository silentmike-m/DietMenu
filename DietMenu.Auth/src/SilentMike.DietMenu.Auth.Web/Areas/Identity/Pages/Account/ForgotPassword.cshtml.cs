namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Pages.Account;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;

public class ForgotPasswordModel : PageModel
{
    private readonly IMediator mediator;
    [BindProperty]
    public ForgotPasswordInputModel Input { get; set; } = new();

    public ForgotPasswordModel(IMediator mediator) => this.mediator = mediator;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        try
        {
            // var request = new ResetPasswordRequest
            // {
            //     Email = this.Input.Email,
            // };
            //
            // await this.mediator.Send(request, CancellationToken.None);

            return this.RedirectToPage("./ForgotPasswordConfirmation");
        }
        catch (Exception exception)
        {
            this.ModelState.AddModelError(string.Empty, exception.Message);

            return this.Page();
        }
    }
}
