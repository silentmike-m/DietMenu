namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Pages.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;
using SilentMike.DietMenu.Auth.Web.Common.Constants;
using SilentMike.DietMenu.Auth.Web.Models;

[AllowAnonymous]
public class ResendEmailConfirmationModel : PageModel
{
    private readonly ILogger<ResendEmailConfirmationModel> logger;
    private readonly ISender mediator;

    [BindProperty] public ResendEmailConfirmationInputModel Input { get; set; } = new();

    public ResendEmailConfirmationModel(ILogger<ResendEmailConfirmationModel> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var request = new GenerateEmailConfirmationToken
            {
                Email = this.Input.Email,
            };

            await this.mediator.Send(request, CancellationToken.None);

            return this.RedirectToPage(IdentityPageNames.LOGIN, new LoginPageValues());
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            this.ModelState.AddModelError(string.Empty, exception.Message);
        }

        return this.Page();
    }
}
