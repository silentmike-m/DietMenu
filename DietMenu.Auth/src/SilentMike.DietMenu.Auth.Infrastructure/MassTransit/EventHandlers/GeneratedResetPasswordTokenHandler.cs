namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.EventHandlers;

using System.Text;
using global::MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Extensions;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.Email.Interfaces;
using SilentMike.DietMenu.Shared.Email.Models;

internal sealed class GeneratedResetPasswordTokenHandler : INotificationHandler<GeneratedResetPasswordToken>
{
    private const int DEFAULT_MESSAGE_EXPIRATION_IN_MINUTES = 30;

    private readonly IActionContextAccessor actionContextAccessor;
    private readonly ICurrentRequestService currentRequestService;
    private readonly ILogger<GeneratedResetPasswordTokenHandler> logger;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly IUrlHelperFactory urlHelperFactory;

    public GeneratedResetPasswordTokenHandler(IActionContextAccessor actionContextAccessor, ICurrentRequestService currentRequestService, ILogger<GeneratedResetPasswordTokenHandler> logger, IPublishEndpoint publishEndpoint, IUrlHelperFactory urlHelperFactory)
    {
        this.actionContextAccessor = actionContextAccessor;
        this.currentRequestService = currentRequestService;
        this.logger = logger;
        this.publishEndpoint = publishEndpoint;
        this.urlHelperFactory = urlHelperFactory;
    }

    public async Task Handle(GeneratedResetPasswordToken notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Sending reset password message");

        var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(notification.Token));

        var url = this.urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext!);
        var returnUrl = url.Content("~/");
        var schema = this.currentRequestService.Schema;
        var callbackUrl = url.Page(
                              "/Account/ResetPassword",
                              pageHandler: null,
                              values: new { area = "Identity", code = token, returnUrl, },
                              protocol: schema)
                          ?? string.Empty;

        var payload = new ResetPasswordEmailPayload
        {
            Email = notification.Email,
            Url = callbackUrl,
        };

        var payloadString = payload.ToJson();

        var payloadType = payload.GetType().FullName
                          ?? string.Empty;

        var message = new EmailDataMessage
        {
            Payload = payloadString,
            PayloadType = payloadType,
        };

        await this.publishEndpoint.Publish<IEmailDataMessage>(
            message,
            context => context.TimeToLive = TimeSpan.FromMinutes(DEFAULT_MESSAGE_EXPIRATION_IN_MINUTES),
            cancellationToken);

        await Task.CompletedTask;
    }
}
