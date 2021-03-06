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

internal sealed class GeneratedEmailConfirmationTokenHandler : INotificationHandler<GeneratedEmailConfirmationToken>
{
    private const int DEFAULT_MESSAGE_EXPIRATION_IN_MINUTES = 30;

    private readonly IActionContextAccessor actionContextAccessor;
    private readonly ICurrentRequestService currentRequestService;
    private readonly ILogger<GeneratedEmailConfirmationTokenHandler> logger;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly IUrlHelperFactory urlHelperFactory;

    public GeneratedEmailConfirmationTokenHandler(IActionContextAccessor actionContextAccessor, ICurrentRequestService currentRequestService, ILogger<GeneratedEmailConfirmationTokenHandler> logger, IPublishEndpoint publishEndpoint, IUrlHelperFactory urlHelperFactory)
    {
        this.actionContextAccessor = actionContextAccessor;
        this.currentRequestService = currentRequestService;
        this.logger = logger;
        this.publishEndpoint = publishEndpoint;
        this.urlHelperFactory = urlHelperFactory;
    }

    public async Task Handle(GeneratedEmailConfirmationToken notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Sending verify account message");

        var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(notification.Token));

        var url = this.urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext!);
        var returnUrl = url.Content("~/");
        var schema = this.currentRequestService.Schema;
        var callbackUrl = url.Page(
                              "/Account/ConfirmEmail",
                              pageHandler: null,
                              values: new { area = "Identity", userId = notification.UserId, code = token, returnUrl, },
                              protocol: schema)
                          ?? string.Empty;

        var payload = new VerifyUserEmailPayload
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
