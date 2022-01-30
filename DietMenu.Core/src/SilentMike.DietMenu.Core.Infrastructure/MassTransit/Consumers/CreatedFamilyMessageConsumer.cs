namespace SilentMike.DietMenu.Core.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Families.Commands;
using SilentMike.DietMenu.Shared.MassTransit;

internal sealed class CreatedFamilyMessageConsumer : IConsumer<ICreatedFamilyMessage>
{
    private readonly ILogger<CreatedFamilyMessageConsumer> logger;
    private readonly IMediator mediator;

    public CreatedFamilyMessageConsumer(ILogger<CreatedFamilyMessageConsumer> logger, IMediator mediator)
        => (this.logger, this.mediator) = (logger, mediator);

    public async Task Consume(ConsumeContext<ICreatedFamilyMessage> context)
    {
        this.logger.LogInformation("Receive verify user message");

        if (context.ExpirationTime.HasValue && DateTime.UtcNow > context.ExpirationTime.Value.ToUniversalTime())
        {
            throw new TimeoutException();
        }

        var command = new CreateFamily
        {
            Id = context.Message.Id,
            Name = context.Message.Name,
        };

        await this.mediator.Send(command, CancellationToken.None);
    }
}
