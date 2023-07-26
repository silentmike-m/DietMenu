namespace SilentMike.DietMenu.Auth.Infrastructure.Users.EventHandlers;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Identity;

internal sealed class CreatedUserHandler : INotificationHandler<CreatedUser>
{
    private readonly IdentityOptions identityOptions;
    private readonly ILogger<CreatedUserHandler> logger;
    private readonly ISender mediator;

    public CreatedUserHandler(IOptions<IdentityOptions> identityOptions, ILogger<CreatedUserHandler> logger, ISender mediator)
    {
        this.identityOptions = identityOptions.Value;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task Handle(CreatedUser notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(
            ("UserId", notification.Id)
        );

        this.logger.LogInformation("User has been created");

        if (this.identityOptions.RequireConfirmedAccount is false)
        {
            this.logger.LogInformation("Require confirmed account has been disabled");

            return;
        }

        var request = new GenerateEmailConfirmationToken
        {
            Id = notification.Id,
        };

        await this.mediator.Send(request, cancellationToken);
    }
}
