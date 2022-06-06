namespace SilentMike.DietMenu.Core.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using SilentMike.DietMenu.Core.Application.Families.Commands;
using SilentMike.DietMenu.Core.Infrastructure.Extensions;
using SilentMike.DietMenu.Shared.Core.Interfaces;
using SilentMike.DietMenu.Shared.Core.Models;

internal sealed class CoreDataMessageConsumer : IConsumer<ICoreDataMessage>
{
    private readonly ILogger<CoreDataMessageConsumer> logger;
    private readonly IMediator mediator;

    public CoreDataMessageConsumer(ILogger<CoreDataMessageConsumer> logger, IMediator mediator)
        => (this.logger, this.mediator) = (logger, mediator);

    public async Task Consume(ConsumeContext<ICoreDataMessage> context)
    {
        this.logger.LogInformation("Received core data message");

        if (context.ExpirationTime.HasValue && DateTime.UtcNow > context.ExpirationTime.Value.ToUniversalTime())
        {
            throw new TimeoutException();
        }

        if (context.Message.PayloadType == typeof(CreatedFamilyPayload).FullName)
        {
            var payload = context.Message.Payload.To<CreatedFamilyPayload>();

            var command = new CreateFamily
            {
                Id = payload.FamilyId,
            };

            _ = await this.mediator.Send(command, CancellationToken.None);
        }
        else
        {
            throw new FormatException("Unsupported core data payload type");
        }
    }
}
