namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Application.Identity.Commands;
using SilentMike.DietMenu.Shared.MassTransit.Identity;

internal sealed class SendResetPasswordMessageRequestConsumer : IConsumer<ISendResetPasswordMessageRequest>
{
    private readonly ILogger<SendResetPasswordMessageRequestConsumer> logger;
    private readonly IMediator mediator;

    public SendResetPasswordMessageRequestConsumer(ILogger<SendResetPasswordMessageRequestConsumer> logger, IMediator mediator)
        => (this.logger, this.mediator) = (logger, mediator);

    public async Task Consume(ConsumeContext<ISendResetPasswordMessageRequest> context)
    {
        this.logger.LogInformation("Received send reset password message request");

        if (context.ExpirationTime.HasValue && DateTime.UtcNow > context.ExpirationTime.Value.ToUniversalTime())
        {
            throw new TimeoutException();
        }

        var command = new SendResetPasswordEmail
        {
            Email = context.Message.Email,
            Url = context.Message.Url,
        };

        await this.mediator.Send(command, CancellationToken.None);
    }
}
