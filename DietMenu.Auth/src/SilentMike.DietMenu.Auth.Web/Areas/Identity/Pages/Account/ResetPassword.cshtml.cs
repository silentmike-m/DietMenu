namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Pages.Account;

using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;
using SilentMike.DietMenu.Auth.Web.Common.Constants;
using SilentMike.DietMenu.Auth.Web.Models;

public class ResetPasswordModel : PageModel
{
    private readonly ILogger<ResetPasswordModel> logger;
    private readonly ISender mediator;

    [BindProperty] public ResetPasswordInputModel Input { get; set; } = new();

    public ResetPasswordModel(ILogger<ResetPasswordModel> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public IActionResult OnGet(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return this.BadRequest("A token must be supplied for password reset.");
        }

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(string token, string returnUrl)
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        try
        {
            var tokenString = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var request = new ResetUserPassword
            {
                Email = this.Input.Email,
                Password = this.Input.Password,
                Token = tokenString,
            };

            await this.mediator.Send(request, CancellationToken.None);

            var pageValues = new ResetPasswordConfirmationPageValues
            {
                ReturnUrl = new Uri(returnUrl),
            };

            return this.RedirectToPage(IdentityPageNames.RESET_PASSWORD_CONFIRMATION, pageValues);
        }
        catch (UserNotFoundException exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            this.ModelState.AddModelError(string.Empty, "Invalid reset password attempt");
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            this.ModelState.AddModelError(string.Empty, exception.Message);
        }

        return this.Page();
    }
}
