namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Pages.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;

[AllowAnonymous]
public class ResendEmailConfirmationModel : PageModel
{
    private readonly IMediator mediator;
    [BindProperty]
    public ResendEmailConfirmationInputModel Input { get; set; } = new();

    public ResendEmailConfirmationModel(IMediator mediator) => this.mediator = mediator;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        try
        {
            // var request = new SendUserConfirmation
            // {
            //     Email = this.Input.Email,
            // };
            //
            // await this.mediator.Send(request, CancellationToken.None);

            return this.RedirectToPage("./Login");
        }
        catch (Exception exception)
        {
            this.ModelState.AddModelError(string.Empty, exception.Message);

            return this.Page();
        }
    }
}
