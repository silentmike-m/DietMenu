namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.MassTransit.Identity;

internal sealed class GetFamilyUserEmailRequestConsumer : IConsumer<IGetFamilyUserEmailRequest>
{
    private readonly ILogger<GetFamilyUserEmailRequestConsumer> logger;
    private readonly UserManager<DietMenuUser> userManager;

    public GetFamilyUserEmailRequestConsumer(ILogger<GetFamilyUserEmailRequestConsumer> logger, UserManager<DietMenuUser> userManager)
        => (this.logger, this.userManager) = (logger, userManager);

    public async Task Consume(ConsumeContext<IGetFamilyUserEmailRequest> context)
    {
        this.logger.LogInformation("Received get family user email request");

        var userId = context.Message.FamilyId.ToString();

        var user = await this.userManager.FindByIdAsync(userId);

        if (user is null)
        {
            throw new UserNotFoundException(context.Message.FamilyId);
        }

        var response = new GetFamilyUserEmailResponse
        {
            Email = user.Email,
        };

        await context.RespondAsync<IGetFamilyUserEmailResponse>(response);
    }
}
