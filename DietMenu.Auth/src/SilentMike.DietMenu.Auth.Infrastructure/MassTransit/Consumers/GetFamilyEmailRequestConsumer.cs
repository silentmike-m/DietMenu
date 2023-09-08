namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common.Extensions;
using SilentMike.DietMenu.Auth.Application.Families.Queries;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.Identity.Interfaces;

internal sealed class GetFamilyEmailRequestConsumer : IConsumer<IGetFamilyEmailRequest>
{
    private readonly ILogger<GetFamilyEmailRequestConsumer> logger;
    private readonly ISender mediator;

    public GetFamilyEmailRequestConsumer(ILogger<GetFamilyEmailRequestConsumer> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IGetFamilyEmailRequest> context)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", context.Message.FamilyId)
        );

        this.logger.LogInformation("Received get family owner request");

        var request = new GetFamilyEmail
        {
            FamilyId = context.Message.FamilyId,
        };

        var familyEmail = await this.mediator.Send(request, context.CancellationToken);

        var response = new GetFamilyEmailResponse
        {
            Email = familyEmail,
            FamilyId = context.Message.FamilyId,
        };

        await context.RespondAsync<IGetFamilyEmailResponse>(response);
    }
}
