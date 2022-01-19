namespace SilentMike.DietMenu.Core.Infrastructure.MassTransit.EventHandlers;
using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Core.Application.Auth.Events;
using SilentMike.DietMenu.Core.Infrastructure.Identity;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.MassTransit;

internal sealed class CreatedEmailConfirmationTokenHandler : INotificationHandler<CreatedEmailConfirmationToken>
{
    private const int DEFAULT_MESSAGE_EXPIRATION_IN_MINUTES = 30;

    private readonly ILogger<CreatedEmailConfirmationTokenHandler> logger;
    private readonly JwtOptions options;
    private readonly IPublishEndpoint publishEndpoint;

    public CreatedEmailConfirmationTokenHandler(
        ILogger<CreatedEmailConfirmationTokenHandler> logger,
        IOptions<JwtOptions> options,
        IPublishEndpoint publishEndpoint)
    {
        this.logger = logger;
        this.options = options.Value;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task Handle(CreatedEmailConfirmationToken notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Sending email confirmation token");

        var url = $"{this.options.Audience}/Account/ConfirmEmail?token={notification.Token}";

        var message = new SendVerifyEmailRequest
        {
            Email = notification.Email,
            Url = url,
            UserName = notification.UserName,
        };

        await this.publishEndpoint.Publish<ISendVerifyEmailRequest>(
            message,
            context => context.TimeToLive = TimeSpan.FromMinutes(DEFAULT_MESSAGE_EXPIRATION_IN_MINUTES),
            cancellationToken);
    }
}
