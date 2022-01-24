namespace SilentMike.DietMenu.Core.Infrastructure.Identity.EventHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Auth.Events;
using SilentMike.DietMenu.Core.Application.Common;

internal sealed class ConfirmedEmailHandler : INotificationHandler<ConfirmedEmail>
{
    private readonly ILogger<ConfirmedEmailHandler> logger;

    public ConfirmedEmailHandler(ILogger<ConfirmedEmailHandler> logger)
        => (this.logger) = (logger);

    public async Task Handle(ConfirmedEmail notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", notification.Email)
        );

        this.logger.LogInformation("Email has been confirmed");

        await Task.CompletedTask;
    }
}
