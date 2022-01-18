namespace SilentMike.DietMenu.Core.Infrastructure.MassTransit.EventHandlers;

using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Users.Events;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.MassTransit;

internal sealed class CreatedUserHandler : INotificationHandler<CreatedUser>
{
    private const int DEFAULT_MESSAGE_EXPIRATION_IN_MINUTES = 30;

    private readonly ILogger<CreatedUserHandler> logger;
    private readonly IPublishEndpoint publishEndpoint;

    public CreatedUserHandler(ILogger<CreatedUserHandler> logger, IPublishEndpoint publishEndpoint)
        => (this.logger, this.publishEndpoint) = (logger, publishEndpoint);

    public async Task Handle(CreatedUser notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Sending created user message");

        var message = new SendCreateUserMessage
        {
            Email = notification.Email,
            FamilyName = notification.FamilyName,
            LoginUrl = notification.LoginUrl,
            UserName = notification.UserName,
        };

        await this.publishEndpoint.Publish<ISendCreatedUserMessage>(
            message,
            context => context.TimeToLive = TimeSpan.FromMinutes(DEFAULT_MESSAGE_EXPIRATION_IN_MINUTES),
            cancellationToken);
    }
}
