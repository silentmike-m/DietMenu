namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Pages.Account;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;

public class RegisterModel : PageModel
{
    private readonly IMediator mediator;
    [BindProperty]
    public RegisterInputModel Input { get; set; } = new();

    public RegisterModel(IMediator mediator) => this.mediator = mediator;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        try
        {
            // var request = new CreateUser
            // {
            //     Email = this.Input.Email,
            //     Family = this.Input.Family,
            //     FirstName = this.Input.FirstName,
            //     LastName = this.Input.LastName,
            //     Password = this.Input.Password,
            //     RegisterCode = this.Input.RegisterCode,
            // };
            //
            // await this.mediator.Send(request, CancellationToken.None);

            return this.Redirect("./RegisterConfirmation");
        }
        catch (Exception exception)
        {
            this.ModelState.AddModelError(string.Empty, exception.Message);

            return this.Page();
        }
    }
}
