namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common.Extensions;
using SilentMike.DietMenu.Auth.Application.Families.Queries;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.Identity.Interfaces;

internal sealed class GetFamilyOwnerEmailRequestConsumer : IConsumer<IGetFamilyOwnerEmailRequest>
{
    private readonly ILogger<GetFamilyOwnerEmailRequestConsumer> logger;
    private readonly ISender mediator;

    public GetFamilyOwnerEmailRequestConsumer(ILogger<GetFamilyOwnerEmailRequestConsumer> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IGetFamilyOwnerEmailRequest> context)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", context.Message.FamilyId)
        );

        this.logger.LogInformation("Received get family owner request");

        var request = new GetFamilyOwner
        {
            FamilyId = context.Message.FamilyId,
        };

        var familyOwner = await this.mediator.Send(request, context.CancellationToken);

        var response = new GetFamilyOwnerEmailResponse
        {
            Email = familyOwner.Email,
            FamilyId = context.Message.FamilyId,
            UserId = familyOwner.UserId,
        };

        await context.RespondAsync<IGetFamilyOwnerEmailResponse>(response);
    }
}
