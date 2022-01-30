namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.EventHandlers;

using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;

internal sealed class CreatedUserHandler : INotificationHandler<CreatedUser>
{
    private readonly ILogger<CreatedUserHandler> logger;
    private readonly IMediator mediator;

    public CreatedUserHandler(ILogger<CreatedUserHandler> logger, IMediator mediator)
        => (this.logger, this.mediator) = (logger, mediator);

    public async Task Handle(CreatedUser notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", notification.Email)
        );

        this.logger.LogInformation("User has been created");

        _ = await this.mediator.Send(new SendUserConfirmation { Email = notification.Email }, cancellationToken);
    }
}
