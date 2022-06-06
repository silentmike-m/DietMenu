namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.EventHandlers;

using global::MassTransit;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Extensions;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.Core.Interfaces;
using SilentMike.DietMenu.Shared.Core.Models;

internal sealed class CreatedFamilyHandler : INotificationHandler<CreatedFamily>
{
    private readonly ILogger<CreatedFamilyHandler> logger;
    private readonly IPublishEndpoint publishEndpoint;

    public CreatedFamilyHandler(ILogger<CreatedFamilyHandler> logger, IPublishEndpoint publishEndpoint)
        => (this.logger, this.publishEndpoint) = (logger, publishEndpoint);

    public async Task Handle(CreatedFamily notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Sending created family message");

        var payload = new CreatedFamilyPayload
        {
            FamilyId = notification.Id,
        };

        var payloadString = payload.ToJson();

        var payloadType = payload.GetType().FullName
                          ?? string.Empty;

        var message = new CoreDataMessage
        {
            Payload = payloadString,
            PayloadType = payloadType,
        };

        await this.publishEndpoint.Publish<ICoreDataMessage>(message, context => context.TimeToLive = TimeSpan.FromMinutes(5), cancellationToken);

        await Task.CompletedTask;
    }
}
