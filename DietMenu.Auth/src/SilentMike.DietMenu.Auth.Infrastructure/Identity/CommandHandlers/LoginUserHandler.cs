namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.CommandHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class LoginUserHandler : IRequestHandler<LoginUser>
{
    private readonly ILogger<LoginUserHandler> logger;
    private readonly SignInManager<DietMenuUser> signInManager;

    public LoginUserHandler(ILogger<LoginUserHandler> logger, SignInManager<DietMenuUser> signInManager)
        => (this.logger, this.signInManager) = (logger, signInManager);

    public async Task<Unit> Handle(LoginUser request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email)
        );
        this.logger.LogInformation("Try to login user");

        var result = await this.signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            this.logger.LogError("IsLockedOut: {Message}", result.IsLockedOut);
            this.logger.LogError("IsNotAllowed: {Message}", result.IsNotAllowed);
            this.logger.LogError("Succeeded: {Message}", result.Succeeded);

            throw new LoginUserException();
        }

        return await Task.FromResult(Unit.Value);
    }
}
