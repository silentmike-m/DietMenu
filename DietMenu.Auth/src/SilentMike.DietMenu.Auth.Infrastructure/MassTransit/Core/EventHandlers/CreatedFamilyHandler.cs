namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Core.EventHandlers;

using global::MassTransit;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common.Extensions;
using SilentMike.DietMenu.Auth.Application.Families.Events;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Core.Models;
using SilentMike.DietMenu.Shared.Core.Interfaces;

internal sealed class CreatedFamilyHandler : INotificationHandler<CreatedFamily>
{
    private readonly IBus bus;
    private readonly ILogger<CreatedFamilyHandler> logger;

    public CreatedFamilyHandler(IBus bus, ILogger<CreatedFamilyHandler> logger)
    {
        this.bus = bus;
        this.logger = logger;
    }

    public async Task Handle(CreatedFamily notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", notification.Id)
        );

        this.logger.LogInformation("Try to send created family message");

        var message = new CreatedFamilyMessage
        {
            Id = notification.Id,
        };

        await this.bus.Publish<ICreatedFamilyMessage>(message, cancellationToken);
    }
}
