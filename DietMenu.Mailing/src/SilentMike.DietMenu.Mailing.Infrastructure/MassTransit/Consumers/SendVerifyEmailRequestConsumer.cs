namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Application.Users.Commands;
using SilentMike.DietMenu.Shared.MassTransit;

internal sealed class SendVerifyEmailRequestConsumer : IConsumer<ISendVerifyEmailRequest>
{
    private readonly ILogger<SendVerifyEmailRequestConsumer> logger;
    private readonly IMediator mediator;

    public SendVerifyEmailRequestConsumer(ILogger<SendVerifyEmailRequestConsumer> logger, IMediator mediator)
        => (this.logger, this.mediator) = (logger, mediator);

    public async Task Consume(ConsumeContext<ISendVerifyEmailRequest> context)
    {
        this.logger.LogInformation("Receive created user message");

        if (context.ExpirationTime.HasValue && DateTime.UtcNow > context.ExpirationTime.Value.ToUniversalTime())
        {
            throw new TimeoutException();
        }

        var command = new SendVerifyEmail
        {
            Email = context.Message.Email,
            Url = context.Message.Url,
            UserName = context.Message.UserName,
        };

        await this.mediator.Send(command, CancellationToken.None);
    }
}
