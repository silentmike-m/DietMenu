namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Pages.Account;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;
using SilentMike.DietMenu.Auth.Web.Common.Constants;
using SilentMike.DietMenu.Auth.Web.Models;

public class ForgotPasswordModel : PageModel
{
    private readonly ILogger<ForgotPasswordModel> logger;
    private readonly ISender mediator;

    [BindProperty] public ForgotPasswordInputModel Input { get; set; } = new();

    public ForgotPasswordModel(ILogger<ForgotPasswordModel> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (this.ModelState.IsValid is false)
        {
            return this.Page();
        }

        try
        {
            var request = new GenerateResetPasswordToken
            {
                Email = this.Input.Email,
            };

            await this.mediator.Send(request, CancellationToken.None);

            return this.RedirectToPage(IdentityPageNames.FORGOT_PASSWORD_CONFIRMATION, new ForgotPasswordConfirmationPageValues());
        }
        catch (UserNotFoundException exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            this.ModelState.AddModelError(string.Empty, "Invalid generate reset password token attempt");
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            this.ModelState.AddModelError(string.Empty, exception.Message);
        }

        return this.Page();
    }
}
