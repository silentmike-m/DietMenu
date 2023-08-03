namespace SilentMike.DietMenu.Auth.Infrastructure.Auth.CommandHandlers;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Auth.Commands;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

[ExcludeFromCodeCoverage]
internal sealed class SignOutHandler : IRequestHandler<SignOut>
{
    private readonly ILogger<SignOutHandler> logger;
    private readonly SignInManager<User> signInManager;

    public SignOutHandler(ILogger<SignOutHandler> logger, SignInManager<User> signInManager)
    {
        this.logger = logger;
        this.signInManager = signInManager;
    }

    public async Task Handle(SignOut request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to sign out");

        await this.signInManager.SignOutAsync();
    }
}
