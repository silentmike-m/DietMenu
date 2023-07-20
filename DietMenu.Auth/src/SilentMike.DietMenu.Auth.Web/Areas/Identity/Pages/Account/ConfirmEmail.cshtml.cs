namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Pages.Account;

using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

public class ConfirmEmailModel : PageModel
{
    private readonly IMediator mediator;

    public ConfirmEmailModel(IMediator mediator)
        => this.mediator = mediator;

    public IActionResult OnGet(string code, Guid userId)
    {
        if (string.IsNullOrEmpty(code))
        {
            return this.BadRequest("A code must be supplied for account confirmation.");
        }

        if (userId == Guid.Empty)
        {
            return this.BadRequest("A userId must be supplied for account confirmation.");
        }

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(string code, Guid userId)
    {
        try
        {
            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            // var request = new ConfirmUser
            // {
            //     Id = userId,
            //     Token = token,
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
