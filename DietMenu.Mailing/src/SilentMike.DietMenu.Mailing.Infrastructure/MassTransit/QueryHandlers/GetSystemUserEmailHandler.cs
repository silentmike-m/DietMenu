namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.QueryHandlers;

using System.Threading;
using System.Threading.Tasks;
using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Application.Identity.Queries;
using SilentMike.DietMenu.Mailing.Infrastructure.Extensions;
using SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.Identity.Interfaces;
using SilentMike.DietMenu.Shared.Identity.Models;

internal sealed class GetSystemUserEmailHandler : IRequestHandler<GetSystemUserEmail, string>
{
    private readonly ILogger<GetSystemUserEmailHandler> logger;
    private readonly IRequestClient<IIdentityDataRequest> requestClient;

    public GetSystemUserEmailHandler(ILogger<GetSystemUserEmailHandler> logger, IRequestClient<IIdentityDataRequest> requestClient)
    {
        this.logger = logger;
        this.requestClient = requestClient;
    }

    public async Task<string> Handle(GetSystemUserEmail request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get system user email");

        var payload = new GetSystemUserEmailPayload();

        var payloadString = payload.ToJson();

        var payloadType = payload.GetType().FullName
                          ?? string.Empty;

        var message = new IdentityDataRequest
        {
            Payload = payloadString,
            PayloadType = payloadType,
        };

        var response = await this.requestClient
            .GetResponse<GetSystemUserEmailResponse>(message, cancellationToken);

        return response.Message.Email;
    }
}
