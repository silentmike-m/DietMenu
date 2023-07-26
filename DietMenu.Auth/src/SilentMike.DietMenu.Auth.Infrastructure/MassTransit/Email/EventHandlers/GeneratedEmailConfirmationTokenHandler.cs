namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Email.EventHandlers;

using global::MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Extensions;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Interfaces;
using SilentMike.DietMenu.Auth.Infrastructure.IdentityServer;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Email.Models;
using SilentMike.DietMenu.Shared.Email.Models;

internal sealed class GeneratedEmailConfirmationTokenHandler : INotificationHandler<GeneratedEmailConfirmationToken>
{
    private readonly IBus bus;
    private readonly IIdentityPageUrlService identityPageUrlService;
    private readonly IdentityServerOptions identityServerOptions;
    private readonly ILogger<GeneratedEmailConfirmationTokenHandler> logger;

    public GeneratedEmailConfirmationTokenHandler(IBus bus, IIdentityPageUrlService identityPageUrlService, IOptions<IdentityServerOptions> identityServerOptions, ILogger<GeneratedEmailConfirmationTokenHandler> logger)
    {
        this.bus = bus;
        this.identityPageUrlService = identityPageUrlService;
        this.identityServerOptions = identityServerOptions.Value;
        this.logger = logger;
    }

    public async Task Handle(GeneratedEmailConfirmationToken notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("UserId", notification.Id)
        );

        this.logger.LogInformation("Try to send confirm user email message");

        var host = new Uri(this.identityServerOptions.IssuerUri);

        var url = this.identityPageUrlService.GetConfirmUserEmailPageUrl(host, notification.Token, notification.Id);

        var payload = new ConfirmUserEmailPayload
        {
            Email = notification.Email,
            Url = url,
        };

        var payloadJson = payload.ToJson();

        var message = new EmailDataMessage
        {
            Payload = payloadJson,
            PayloadType = typeof(ConfirmUserEmailPayload).FullName!,
        };

        await this.bus.Publish(message, cancellationToken);
    }
}
