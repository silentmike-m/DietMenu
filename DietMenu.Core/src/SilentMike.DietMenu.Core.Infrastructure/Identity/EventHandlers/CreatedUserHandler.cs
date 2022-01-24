namespace SilentMike.DietMenu.Core.Infrastructure.Identity.EventHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Auth.Commands;
using SilentMike.DietMenu.Core.Application.Auth.Events;
using SilentMike.DietMenu.Core.Application.Common;

internal sealed class CreatedUserHandler : INotificationHandler<CreatedUser>
{
    private readonly ILogger<CreatedUserHandler> logger;
    private readonly IMediator mediator;

    public CreatedUserHandler(ILogger<CreatedUserHandler> logger, IMediator mediator)
        => (this.logger, this.mediator) = (logger, mediator);

    public async Task Handle(CreatedUser notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyName", notification.FamilyName),
            ("UserName", notification.UserName)
        );

        this.logger.LogInformation("User has been created");

        var command = new CreateEmailConfirmationToken
        {
            Email = notification.Email,
        };

        _ = await this.mediator.Send(command, cancellationToken);
    }
}
