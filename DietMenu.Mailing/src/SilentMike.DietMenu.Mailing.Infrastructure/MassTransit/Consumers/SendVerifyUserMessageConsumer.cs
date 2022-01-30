namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Application.Users.Commands;
using SilentMike.DietMenu.Shared.MassTransit;

internal sealed class SendVerifyUserMessageConsumer : IConsumer<ISendVerifyUserMessage>
{
    private readonly ILogger<SendVerifyUserMessageConsumer> logger;
    private readonly IMediator mediator;

    public SendVerifyUserMessageConsumer(ILogger<SendVerifyUserMessageConsumer> logger, IMediator mediator)
        => (this.logger, this.mediator) = (logger, mediator);

    public async Task Consume(ConsumeContext<ISendVerifyUserMessage> context)
    {
        this.logger.LogInformation("Receive verify user message");

        if (context.ExpirationTime.HasValue && DateTime.UtcNow > context.ExpirationTime.Value.ToUniversalTime())
        {
            throw new TimeoutException();
        }

        var command = new SendVerifyUserEmail
        {
            Email = context.Message.Email,
            Url = context.Message.Url,
        };

        await this.mediator.Send(command, CancellationToken.None);
    }
}
