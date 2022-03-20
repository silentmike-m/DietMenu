namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.QueryHandlers;

using System.Threading;
using System.Threading.Tasks;
using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Application.Identity.Queries;
using SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.MassTransit.Identity;

internal sealed class GetSystemUserEmailHandler : IRequestHandler<GetSystemUserEmail, string>
{
    private readonly ILogger<GetSystemUserEmailHandler> logger;
    private readonly IRequestClient<IGetSystemUserEmailRequest> requestClient;

    public GetSystemUserEmailHandler(
        ILogger<GetSystemUserEmailHandler> logger,
        IRequestClient<IGetSystemUserEmailRequest> requestClient)
    {
        this.logger = logger;
        this.requestClient = requestClient;
    }

    public async Task<string> Handle(GetSystemUserEmail request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get system user email");

        var getSystemUserEmailRequest = new GetSystemUserEmailRequest();

        var response = await this.requestClient
            .GetResponse<IGetSystemUserEmailResponse>(getSystemUserEmailRequest, cancellationToken);

        return response.Message.Email;
    }
}
