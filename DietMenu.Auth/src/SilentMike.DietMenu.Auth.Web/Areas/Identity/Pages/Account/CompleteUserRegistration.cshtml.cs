namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Pages.Account;

using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;
using SilentMike.DietMenu.Auth.Web.Common.Constants;

public class CompleteUserRegistration : PageModel
{
    private readonly ILogger<CompleteUserRegistration> logger;
    private readonly IMediator mediator;

    [BindProperty] public CompleteUserRegistrationInputModel Input { get; set; } = new();

    public CompleteUserRegistration(IMediator mediator, ILogger<CompleteUserRegistration> logger)
    {
        this.mediator = mediator;
        this.logger = logger;
    }

    public Task<IActionResult> OnGet(string token, Guid userId)
    {
        if (string.IsNullOrEmpty(token))
        {
            return Task.FromResult<IActionResult>(this.BadRequest("A token must be supplied for account confirmation."));
        }

        if (userId == Guid.Empty)
        {
            return Task.FromResult<IActionResult>(this.BadRequest("A userId must be supplied for account confirmation."));
        }

        return Task.FromResult<IActionResult>(this.Page());
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl, string token, Guid userId)
    {
        if (this.ModelState.IsValid is false)
        {
            return this.Page();
        }

        try
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = IdentityPageNames.LOGIN;
            }

            var tokenString = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var request = new Application.Users.Commands.CompleteUserRegistration
            {
                Id = userId,
                Password = this.Input.Password,
                Token = tokenString,
            };

            await this.mediator.Send(request, CancellationToken.None);

            return this.Redirect(returnUrl);
        }
        catch (UserNotFoundException exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            this.ModelState.AddModelError(string.Empty, "Invalid complete registration attempt");
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            this.ModelState.AddModelError(string.Empty, exception.Message);
        }

        return this.Page();
    }
}
