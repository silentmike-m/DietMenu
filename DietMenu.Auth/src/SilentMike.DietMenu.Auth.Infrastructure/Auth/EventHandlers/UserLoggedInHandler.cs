namespace SilentMike.DietMenu.Auth.Infrastructure.Auth.EventHandlers;

using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Auth.Events;
using SilentMike.DietMenu.Auth.Application.Common.Extensions;
using SilentMike.DietMenu.Auth.Application.Users.Commands;

internal sealed class UserLoggedInHandler : INotificationHandler<UserLoggedIn>
{
    private readonly ILogger<UserLoggedInHandler> logger;
    private readonly ISender mediator;

    public UserLoggedInHandler(ILogger<UserLoggedInHandler> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task Handle(UserLoggedIn notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("UserId", notification.UserId)
        );

        this.logger.LogInformation("User logged in");

        var request = new UpdateUserLastLoginDate
        {
            UserId = notification.UserId,
        };

        await this.mediator.Send(request, cancellationToken);
    }
}
