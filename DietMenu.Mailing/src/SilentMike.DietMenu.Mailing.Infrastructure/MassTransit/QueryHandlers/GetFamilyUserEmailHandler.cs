namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.QueryHandlers;

using global::MassTransit;
using SilentMike.DietMenu.Mailing.Application.Identity.Queries;
using SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.Identity.Interfaces;

internal sealed class GetFamilyUserEmailHandler : IRequestHandler<GetFamilyUserEmail, string>
{
    private readonly ILogger<GetFamilyUserEmailHandler> logger;
    private readonly IRequestClient<IGetFamilyUserEmailRequest> requestClient;

    public GetFamilyUserEmailHandler(ILogger<GetFamilyUserEmailHandler> logger, IRequestClient<IGetFamilyUserEmailRequest> requestClient)
    {
        this.logger = logger;
        this.requestClient = requestClient;
    }

    public async Task<string> Handle(GetFamilyUserEmail request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get family user email");

        var message = new GetFamilyUserEmailRequest
        {
            FamilyId = request.FamilyId,
        };

        var response = await this.requestClient.GetResponse<IGetFamilyUserEmailResponse>(message, cancellationToken);

        return response.Message.Email;
    }
}
