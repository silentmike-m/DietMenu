namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Application.Identity.Commands;
using SilentMike.DietMenu.Shared.MassTransit.Identity;

internal sealed class SendVerifyUserMessageRequestConsumer : IConsumer<ISendVerifyUserMessageRequest>
{
    private readonly ILogger<SendVerifyUserMessageRequestConsumer> logger;
    private readonly IMediator mediator;

    public SendVerifyUserMessageRequestConsumer(ILogger<SendVerifyUserMessageRequestConsumer> logger, IMediator mediator)
        => (this.logger, this.mediator) = (logger, mediator);

    public async Task Consume(ConsumeContext<ISendVerifyUserMessageRequest> context)
    {
        this.logger.LogInformation("Received send verify user message request");

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
