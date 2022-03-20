namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.QueryHandlers;

using System.Threading;
using System.Threading.Tasks;
using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Application.Identity.Queries;
using SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.MassTransit.Identity;

internal sealed class GetFamilyUserEmailHandler : IRequestHandler<GetFamilyUserEmail, string>
{
    private readonly ILogger<GetFamilyUserEmailHandler> logger;
    private readonly IRequestClient<IGetFamilyUserEmailRequest> requestClient;

    public GetFamilyUserEmailHandler(
        ILogger<GetFamilyUserEmailHandler> logger,
        IRequestClient<IGetFamilyUserEmailRequest> requestClient)
    {
        this.logger = logger;
        this.requestClient = requestClient;
    }

    public async Task<string> Handle(GetFamilyUserEmail request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get family user email");

        var getFamilyUserEmailRequest = new GetFamilyUserEmailRequest
        {
            FamilyId = request.FamilyId,
        };

        var response = await this.requestClient
            .GetResponse<IGetFamilyUserEmailResponse>(getFamilyUserEmailRequest, cancellationToken);

        return response.Message.Email;
    }
}
