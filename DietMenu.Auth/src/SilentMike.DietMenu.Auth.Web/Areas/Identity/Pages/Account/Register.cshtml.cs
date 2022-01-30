namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Pages.Account;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Web.Areas.Identity.Models;

public class RegisterModel : PageModel
{
    [BindProperty]
    public RegisterInputModel Input { get; set; } = new();

    private readonly IMediator mediator;

    public RegisterModel(IMediator mediator) => this.mediator = mediator;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        try
        {
            var request = new CreateUser
            {
                Email = this.Input.Email,
                Family = this.Input.Family,
                FirstName = this.Input.FirstName,
                LastName = this.Input.LastName,
                Password = this.Input.Password,
                RegisterCode = this.Input.RegisterCode,
            };

            _ = await this.mediator.Send(request, CancellationToken.None);

            return this.Redirect("./RegisterConfirmation");
        }
        catch (Exception exception)
        {
            this.ModelState.AddModelError(string.Empty, exception.Message);
            return this.Page();
        }
    }
}