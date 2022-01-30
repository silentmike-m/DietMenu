namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.CommandHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class ResetPasswordRequestHandler : IRequestHandler<ResetPasswordRequest>
{
    private readonly ILogger<ResetPasswordRequestHandler> logger;
    private readonly IMediator mediator;
    private readonly UserManager<DietMenuUser> userManager;

    public ResetPasswordRequestHandler(ILogger<ResetPasswordRequestHandler> logger, IMediator mediator, UserManager<DietMenuUser> userManager)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.userManager = userManager;
    }

    public async Task<Unit> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email)
        );
        this.logger.LogInformation("Try to generate rest password token");

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            this.logger.LogError("User not found");
            return await Task.FromResult(Unit.Value);
        }

        var isEmailConfirmed = await this.userManager.IsEmailConfirmedAsync(user);

        if (!isEmailConfirmed)
        {
            this.logger.LogError("Email not confirmed");
            return await Task.FromResult(Unit.Value);
        }

        var token = await this.userManager.GeneratePasswordResetTokenAsync(user);

        var notification = new GeneratedResetPasswordToken
        {
            Email = user.Email,
            Token = token,
        };

        await this.mediator.Publish(notification, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }
}
