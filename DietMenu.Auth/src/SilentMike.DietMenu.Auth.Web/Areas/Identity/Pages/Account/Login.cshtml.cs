namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Pages.Account;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilentMike.DietMenu.Auth.Application.Auth.Commands;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;
using SilentMike.DietMenu.Auth.Web.Common.Constants;
using SilentMike.DietMenu.Auth.Web.Interfaces;

public sealed class LoginModel : PageModel
{
    private readonly IHttpContextSignInService httpContextSignInService;
    private readonly ILogger<LoginModel> logger;
    private readonly ISender mediator;

    [BindProperty] public LoginInputModel Input { get; set; } = new();

    public LoginModel(IHttpContextSignInService httpContextSignInService, ILogger<LoginModel> logger, ISender mediator)
    {
        this.httpContextSignInService = httpContextSignInService;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task<IActionResult> OnGetAsync(string? returnUrl = default)
    {
        returnUrl ??= this.Url.Content(IdentityPageNames.HOME);

        await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        this.Input.ReturnUrl = returnUrl;

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null, CancellationToken cancellationToken = default)
    {
        if (this.ModelState.IsValid is false)
        {
            return this.Page();
        }

        try
        {
            var passwordSignInUser = new PasswordSignInUser
            {
                Email = this.Input.Email,
                Password = this.Input.Password,
                Remember = this.Input.RememberMe,
            };

            await this.mediator.Send(passwordSignInUser, cancellationToken);

            await this.httpContextSignInService.SignInAsync(this.Input.Email, cancellationToken);

            return this.LocalRedirect(this.Input.ReturnUrl);
        }
        catch (UserNotFoundException exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            this.ModelState.AddModelError(string.Empty, "Invalid login attempt");
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            this.ModelState.AddModelError(string.Empty, exception.Message);
        }

        return this.Page();
    }
}
