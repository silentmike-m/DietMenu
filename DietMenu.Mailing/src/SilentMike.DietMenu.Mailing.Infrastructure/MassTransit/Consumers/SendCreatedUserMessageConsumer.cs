namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Application.Users.Commands;
using SilentMike.DietMenu.Shared.MassTransit;

internal sealed class SendCreatedUserMessageConsumer : IConsumer<ISendCreatedUserMessage>
{
    private readonly ILogger<SendCreatedUserMessageConsumer> logger;
    private readonly IMediator mediator;

    public SendCreatedUserMessageConsumer(ILogger<SendCreatedUserMessageConsumer> logger, IMediator mediator)
        => (this.logger, this.mediator) = (logger, mediator);

    public async Task Consume(ConsumeContext<ISendCreatedUserMessage> context)
    {
        this.logger.LogInformation("Receive created user message");

        if (context.ExpirationTime.HasValue && DateTime.UtcNow > context.ExpirationTime.Value.ToUniversalTime())
        {
            throw new TimeoutException();
        }

        var command = new SendCreatedUserEmail
        {
            Email = context.Message.Email,
            FamilyName = context.Message.FamilyName,
            LoginUrl = context.Message.LoginUrl,
            UserName = context.Message.UserName,
        };

        await this.mediator.Send(command, CancellationToken.None);
    }
}
