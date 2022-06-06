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

internal sealed class GetFamilyUserEmailHandler : IRequestHandler<GetFamilyUserEmail, string>
{
    private readonly ILogger<GetFamilyUserEmailHandler> logger;
    private readonly IRequestClient<IIdentityDataRequest> requestClient;

    public GetFamilyUserEmailHandler(ILogger<GetFamilyUserEmailHandler> logger, IRequestClient<IIdentityDataRequest> requestClient)
        => (this.logger, this.requestClient) = (logger, requestClient);

    public async Task<string> Handle(GetFamilyUserEmail request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get family user email");

        var payload = new GetFamilyUserEmailPayload
        {
            FamilyId = request.FamilyId,
        };

        var payloadString = payload.ToJson();

        var payloadType = payload.GetType().FullName
                          ?? string.Empty;

        var message = new IdentityDataRequest
        {
            Payload = payloadString,
            PayloadType = payloadType,
        };

        var response = await this.requestClient
            .GetResponse<GetFamilyUserEmailResponse>(message, cancellationToken);

        return response.Message.Email;
    }
}
