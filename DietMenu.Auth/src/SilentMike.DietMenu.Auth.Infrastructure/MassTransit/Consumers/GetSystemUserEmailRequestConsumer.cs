namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Auth.Infrastructure.Identity;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.MassTransit.Identity;

internal sealed class GetSystemUserEmailRequestConsumer : IConsumer<IGetSystemUserEmailRequest>
{
    private readonly IdentityOptions identityOptions;
    private readonly ILogger<GetSystemUserEmailRequestConsumer> logger;

    public GetSystemUserEmailRequestConsumer(IOptions<IdentityOptions> identityOptions, ILogger<GetSystemUserEmailRequestConsumer> logger)
    {
        this.identityOptions = identityOptions.Value;
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<IGetSystemUserEmailRequest> context)
    {
        this.logger.LogInformation("Received get system user email request");

        var response = new GetSystemUserEmailResponse
        {
            Email = this.identityOptions.SystemUserEmail,
        };

        await context.RespondAsync<IGetSystemUserEmailResponse>(response);
    }
}
