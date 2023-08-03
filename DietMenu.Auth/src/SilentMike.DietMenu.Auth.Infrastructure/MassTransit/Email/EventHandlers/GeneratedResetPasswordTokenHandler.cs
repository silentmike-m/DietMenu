namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Email.EventHandlers;

using System.Text;
using global::MassTransit;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Auth.Application.Common.Extensions;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Extensions;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Interfaces;
using SilentMike.DietMenu.Auth.Infrastructure.IdentityServer;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Email.Models;
using SilentMike.DietMenu.Shared.Email.Models;

internal sealed class GeneratedResetPasswordTokenHandler : INotificationHandler<GeneratedResetPasswordToken>
{
    private readonly IBus bus;
    private readonly IIdentityPageUrlService identityPageUrlService;
    private readonly IdentityServerOptions identityServerOptions;
    private readonly ILogger<GeneratedResetPasswordTokenHandler> logger;

    public GeneratedResetPasswordTokenHandler(IBus bus, IIdentityPageUrlService identityPageUrlService, IOptions<IdentityServerOptions> identityServerOptions, ILogger<GeneratedResetPasswordTokenHandler> logger)
    {
        this.bus = bus;
        this.identityPageUrlService = identityPageUrlService;
        this.identityServerOptions = identityServerOptions.Value;
        this.logger = logger;
    }

    public async Task Handle(GeneratedResetPasswordToken notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("UserId", notification.Id)
        );

        this.logger.LogInformation("Try to send reset user password email message");

        var hostUri = new Uri(this.identityServerOptions.IssuerUri);

        var returnHostUri = new Uri(this.identityServerOptions.DefaultClientUri);

        var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(notification.Token));

        var url = this.identityPageUrlService.GetResetUserPasswordPageUrl(hostUri, returnHostUri, token);

        var payload = new ResetUserPasswordEmailPayload
        {
            Email = notification.Email,
            Url = url,
        };

        var payloadJson = payload.ToJson();

        var message = new EmailDataMessage
        {
            Payload = payloadJson,
            PayloadType = typeof(ResetUserPasswordEmailPayload).FullName!,
        };

        await this.bus.Publish(message, cancellationToken);
    }
}
