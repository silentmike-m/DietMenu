namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Application.Users.Commands;
using SilentMike.DietMenu.Shared.MassTransit;

internal sealed class SendResetPasswordMessageConsumer : IConsumer<ISendResetPasswordMessage>
{
    private readonly ILogger<SendResetPasswordMessageConsumer> logger;
    private readonly IMediator mediator;

    public SendResetPasswordMessageConsumer(ILogger<SendResetPasswordMessageConsumer> logger, IMediator mediator)
        => (this.logger, this.mediator) = (logger, mediator);

    public async Task Consume(ConsumeContext<ISendResetPasswordMessage> context)
    {
        this.logger.LogInformation("Receive reset password message");

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
