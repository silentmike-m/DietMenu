namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using SilentMike.DietMenu.Mailing.Application.Identity.Commands;
using SilentMike.DietMenu.Mailing.Infrastructure.Extensions;
using SilentMike.DietMenu.Shared.Email.Interfaces;
using SilentMike.DietMenu.Shared.Email.Models;

internal sealed class EmailDataMessageConsumer : IConsumer<IEmailDataMessage>
{
    private readonly ILogger<EmailDataMessageConsumer> logger;
    private readonly ISender mediator;

    public EmailDataMessageConsumer(ILogger<EmailDataMessageConsumer> logger, ISender mediator)
        => (this.logger, this.mediator) = (logger, mediator);

    public async Task Consume(ConsumeContext<IEmailDataMessage> context)
    {
        this.logger.LogInformation("Received email data message");

        if (context.ExpirationTime.HasValue && DateTime.UtcNow > context.ExpirationTime.Value.ToUniversalTime())
        {
            throw new TimeoutException();
        }

        if (context.Message.PayloadType == typeof(ResetUserPasswordEmailPayload).FullName)
        {
            var payload = context.Message.Payload.To<ResetUserPasswordEmailPayload>();

            var command = new SendResetPasswordEmail
            {
                Email = payload.Email,
                Url = payload.Url,
            };

            await this.mediator.Send(command, CancellationToken.None);
        }
        else if (context.Message.PayloadType == typeof(ConfirmUserEmailPayload).FullName)
        {
            var payload = context.Message.Payload.To<ConfirmUserEmailPayload>();

            var command = new SendVerifyUserEmail
            {
                Email = payload.Email,
                Url = payload.Url,
            };

            await this.mediator.Send(command, CancellationToken.None);
        }
        else
        {
            throw new FormatException("Unsupported email data payload type");
        }
    }
}
