namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Pages.Account;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;

public class LoginModel : PageModel
{
    [BindProperty] public LoginInputModel Input { get; set; } = new();

    [TempData] private string ErrorMessage { get; set; }

    private readonly IMediator mediator;

    public LoginModel(IMediator mediator) => this.mediator = mediator;

    public async Task OnGetAsync()
    {
        if (!string.IsNullOrEmpty(this.ErrorMessage))
        {
            this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
        }

        await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        try
        {
            returnUrl ??= this.Url.Content("~/");

            var request = new LoginUser
            {
                Email = this.Input.Email,
                Password = this.Input.Password,
            };

            _ = await this.mediator.Send(request, CancellationToken.None);

            return this.LocalRedirect(returnUrl);
        }
        catch (Exception exception)
        {
            this.ModelState.AddModelError(string.Empty, exception.Message);
            return this.Page();
        }
    }
}