namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Pages.Account;

using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

public class LogoutModel : PageModel
{
    public string? ClientName { get; set; } = default;
    public string? PostLogoutRedirectUri { get; set; } = default;
    public string? SignOutIframeUrl { get; set; } = default;


    private readonly ILogger<LogoutModel> logger;
    private readonly IIdentityServerInteractionService service;
    private readonly SignInManager<DietMenuUser> signInManager;

    public LogoutModel(ILogger<LogoutModel> logger, IIdentityServerInteractionService service, SignInManager<DietMenuUser> signInManager)
    {
        this.logger = logger;
        this.service = service;
        this.signInManager = signInManager;
    }

    public async Task<IActionResult> OnGetAsync(string? logoutId)
    {
        logoutId ??= await this.service.CreateLogoutContextAsync();

        await this.signInManager.SignOutAsync();

        await this.HttpContext.SignOutAsync();

        var logoutContext = await this.service.GetLogoutContextAsync(logoutId);

        this.logger.LogInformation("User logged out.");

        this.ClientName = string.IsNullOrEmpty(logoutContext?.ClientName)
            ? logoutContext?.ClientId
            : logoutContext.ClientName;
        this.PostLogoutRedirectUri = logoutContext?.PostLogoutRedirectUri;
        this.SignOutIframeUrl = logoutContext?.SignOutIFrameUrl;

        return this.Page();
    }
}
