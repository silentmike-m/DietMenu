namespace SilentMike.DietMenu.Core.Infrastructure.MassTransit.Families.Consumers;

using global::MassTransit;
using SilentMike.DietMenu.Core.Application.Common.Extensions;
using SilentMike.DietMenu.Core.Application.Families.Commands;
using SilentMike.DietMenu.Shared.Core.Interfaces;

internal sealed class CreatedFamilyMessageConsumer : IConsumer<ICreatedFamilyMessage>
{
    private readonly ILogger<CreatedFamilyMessageConsumer> logger;
    private readonly ISender mediator;

    public CreatedFamilyMessageConsumer(ILogger<CreatedFamilyMessageConsumer> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ICreatedFamilyMessage> context)
    {
        using var loggerScope = this.logger.BeginPropertyScope("FamilyId", context.Message.Id);

        this.logger.LogInformation("Received created family message");

        var request = new ImportFamilyData
        {
            FamilyId = context.Message.Id,
        };

        await this.mediator.Send(request, context.CancellationToken);
    }
}
