namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.CommandHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class SendUserConfirmationHandler : IRequestHandler<SendUserConfirmation>
{
    private readonly ILogger<SendUserConfirmationHandler> logger;
    private readonly IMediator mediator;
    private readonly UserManager<DietMenuUser> userManager;

    public SendUserConfirmationHandler(
        ILogger<SendUserConfirmationHandler> logger,
        IMediator mediator,
        UserManager<DietMenuUser> userManager)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.userManager = userManager;
    }

    public async Task<Unit> Handle(SendUserConfirmation request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email)
        );
        this.logger.LogInformation("Try to generate email confirmation token");

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            this.logger.LogError("User not found");
            return await Task.FromResult(Unit.Value);
        }

        var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

        var userId = Guid.Parse(user.Id);

        var notification = new GeneratedEmailConfirmationToken
        {
            Email = user.Email,
            Token = token,
            UserId = userId,
        };

        await this.mediator.Publish(notification, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }
}
