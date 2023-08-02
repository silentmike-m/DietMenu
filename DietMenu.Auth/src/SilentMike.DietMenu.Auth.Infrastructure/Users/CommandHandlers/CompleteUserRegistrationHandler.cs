namespace SilentMike.DietMenu.Auth.Infrastructure.Users.CommandHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common.Extensions;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Infrastructure.Exceptions.Users;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class CompleteUserRegistrationHandler : IRequestHandler<CompleteUserRegistration>
{
    private readonly ILogger<CompleteUserRegistrationHandler> logger;
    private readonly UserManager<User> userManager;

    public CompleteUserRegistrationHandler(ILogger<CompleteUserRegistrationHandler> logger, UserManager<User> userManager)
    {
        this.logger = logger;
        this.userManager = userManager;
    }

    public async Task Handle(CompleteUserRegistration request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("UserId", request.Id)
        );

        this.logger.LogInformation("Try to complete user registration");

        var userIdString = request.Id.ToString();

        var user = await this.userManager.FindByIdAsync(userIdString);

        if (user is null)
        {
            throw new UserNotFoundException(request.Id);
        }

        var confirmEmailResult = await this.userManager.ConfirmEmailAsync(user, request.Token);

        if (confirmEmailResult.Succeeded is false)
        {
            throw new ConfirmUserEmailException(request.Id, confirmEmailResult.Errors.First().Description);
        }

        var resetPasswordToken = await this.userManager.GeneratePasswordResetTokenAsync(user);

        var resetPasswordResult = await this.userManager.ResetPasswordAsync(user, resetPasswordToken, request.Password);

        if (resetPasswordResult.Succeeded is false)
        {
            throw new ResetUserPasswordException(request.Id, resetPasswordResult.Errors.First().Description);
        }
    }
}
