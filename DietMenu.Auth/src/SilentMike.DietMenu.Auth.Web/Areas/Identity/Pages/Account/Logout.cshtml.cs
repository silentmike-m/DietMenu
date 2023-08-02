namespace SilentMike.DietMenu.Auth.Web.Areas.Identity.Pages.Account;

using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilentMike.DietMenu.Auth.Application.Auth.Commands;

[AllowAnonymous]
public class LogoutModel : PageModel
{
    private readonly IClientStore clientStore;
    private readonly IIdentityServerInteractionService identityServerService;
    private readonly ILogger<LogoutModel> logger;
    private readonly ISender mediator;

    public string? ClientName { get; set; } = default;
    public List<string> FrontChannelLogoutUris { get; set; } = new();
    public string? PostLogoutRedirectUri { get; set; } = default;

    public LogoutModel(IClientStore clientStore, IIdentityServerInteractionService identityServerService, ILogger<LogoutModel> logger, ISender mediator)
    {
        this.clientStore = clientStore;
        this.identityServerService = identityServerService;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task<IActionResult> OnGetAsync(string? logoutId, CancellationToken cancellationToken = default)
    {
        logoutId ??= await this.identityServerService.CreateLogoutContextAsync();

        var request = new SignOut();

        await this.mediator.Send(request, cancellationToken);

        await this.HttpContext.SignOutAsync();

        this.logger.LogInformation("User logged out.");

        var logoutRequest = await this.identityServerService.GetLogoutContextAsync(logoutId);

        if (logoutRequest is not null)
        {
            this.ClientName = logoutRequest.ClientName;

            this.PostLogoutRedirectUri = logoutRequest.PostLogoutRedirectUri;

            this.FrontChannelLogoutUris = await this.GetFrontChannelLogoutUris(logoutRequest);
        }

        return this.Page();
    }

    private async Task<List<string>> GetFrontChannelLogoutUris(LogoutRequest logoutRequest)
    {
        var frontChannelLogoutUris = new List<string>();

        if (logoutRequest.ClientIds is not null)
        {
            foreach (var clientId in logoutRequest.ClientIds)
            {
                var client = await this.clientStore.FindClientByIdAsync(clientId);

                if (string.IsNullOrEmpty(client?.FrontChannelLogoutUri) is false)
                {
                    frontChannelLogoutUris.Add(client.FrontChannelLogoutUri);
                }
            }
        }

        return frontChannelLogoutUris;
    }
}
