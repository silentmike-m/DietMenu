namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.QueryHandlers;

using global::MassTransit;
using SilentMike.DietMenu.Mailing.Application.Identity.Queries;
using SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.Identity.Interfaces;

internal sealed class GetFamilyEmailHandler : IRequestHandler<GetFamilyEmail, string>
{
    private readonly ILogger<GetFamilyEmailHandler> logger;
    private readonly IRequestClient<IGetFamilyEmailRequest> requestClient;

    public GetFamilyEmailHandler(ILogger<GetFamilyEmailHandler> logger, IRequestClient<IGetFamilyEmailRequest> requestClient)
    {
        this.logger = logger;
        this.requestClient = requestClient;
    }

    public async Task<string> Handle(GetFamilyEmail request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get family email");

        var message = new GetFamilyEmailRequest
        {
            FamilyId = request.FamilyId,
        };

        var response = await this.requestClient.GetResponse<IGetFamilyEmailResponse>(message, cancellationToken);

        return response.Message.Email;
    }
}
