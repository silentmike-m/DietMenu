﻿namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.EventHandlers;

using global::MassTransit;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.MassTransit;

internal sealed class CreatedFamilyHandler : INotificationHandler<CreatedFamily>
{
    private readonly ILogger<CreatedFamilyHandler> logger;
    private readonly IPublishEndpoint publishEndpoint;

    public CreatedFamilyHandler(ILogger<CreatedFamilyHandler> logger, IPublishEndpoint publishEndpoint)
        => (this.logger, this.publishEndpoint) = (logger, publishEndpoint);

    public async Task Handle(CreatedFamily notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Sending created family message");

        var message = new CreatedFamilyMessage
        {
            Id = notification.Id,
            Name = notification.Name,
        };

        await this.publishEndpoint.Publish<ICreatedFamilyMessage>(message, cancellationToken);
    }
}