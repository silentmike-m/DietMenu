namespace SilentMike.DietMenu.Auth.Infrastructure.Auth.CommandHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Auth.Commands;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Infrastructure.Exceptions.Auth;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class PasswordSignInUserHandler : IRequestHandler<PasswordSignInUser>
{
    private const bool DEFAULT_LOCK_OUT_ON_FAILURE = false;

    private readonly ILogger<PasswordSignInUserHandler> logger;
    private readonly SignInManager<User> signInManager;

    public PasswordSignInUserHandler(ILogger<PasswordSignInUserHandler> logger, SignInManager<User> signInManager)
    {
        this.logger = logger;
        this.signInManager = signInManager;
    }

    public async Task Handle(PasswordSignInUser request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to password sign in user");

        var user = await this.signInManager.UserManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new UserNotFoundException(request.Email);
        }

        var result = await this.signInManager.PasswordSignInAsync(
            request.Email,
            request.Password,
            request.Remember,
            DEFAULT_LOCK_OUT_ON_FAILURE);

        if (result.Succeeded is false)
        {
            this.LogInvalidLoginAttempt(result);

            throw new InvalidLoginAttemptException();
        }
    }

    private void LogInvalidLoginAttempt(SignInResult result)
    {
        if (result.IsLockedOut)
        {
            this.logger.LogError("Is locked out");
        }
        else if (result.IsNotAllowed)
        {
            this.logger.LogError("Is not allowed");
        }
        else if (result.RequiresTwoFactor)
        {
            this.logger.LogError("Requires two factor");
        }
    }
}
