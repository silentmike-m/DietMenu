namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using SilentMike.DietMenu.Mailing.Application.Family.Commands;
using SilentMike.DietMenu.Mailing.Application.Family.Models;
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
            await this.HandleResetPassword(context);
        }
        else if (context.Message.PayloadType == typeof(ConfirmUserEmailPayload).FullName)
        {
            await this.HandleVerifyUserEmail(context);
        }
        else if (context.Message.PayloadType == typeof(ImportedFamilyDataPayload).FullName)
        {
            await this.HandleImportedFamilyData(context);
        }
        else
        {
            throw new FormatException("Unsupported email data payload type");
        }
    }

    private async Task HandleImportedFamilyData(ConsumeContext<IEmailDataMessage> context)
    {
        var payload = context.Message.Payload.To<ImportedFamilyDataPayload>();

        var results = payload.Results.Select(result =>
            new ImportedFamilyDataResult
            {
                DataArea = result.DataArea,
                Errors = result.Errors.Select(error =>
                    new ImportedFamilyDataError
                    {
                        Code = error.Code,
                        Message = error.Message,
                    }
                ).ToList(),
            }
        ).ToList();

        var request = new SendImportedFamilyDataEmail
        {
            ErrorCode = payload.ErrorCode,
            ErrorMessage = payload.ErrorMessage,
            IsSuccess = payload.ErrorCode is null && results.TrueForAll(result => !result.Errors.Any()),
            FamilyId = payload.FamilyId,
            Results = results,
        };

        await this.mediator.Send(request, context.CancellationToken);
    }

    private async Task HandleResetPassword(ConsumeContext<IEmailDataMessage> context)
    {
        var payload = context.Message.Payload.To<ResetUserPasswordEmailPayload>();

        var request = new SendResetPasswordEmail
        {
            Email = payload.Email,
            Url = payload.Url,
        };

        await this.mediator.Send(request, context.CancellationToken);
    }

    private async Task HandleVerifyUserEmail(ConsumeContext<IEmailDataMessage> context)
    {
        var payload = context.Message.Payload.To<ConfirmUserEmailPayload>();

        var request = new SendVerifyUserEmail
        {
            Email = payload.Email,
            Url = payload.Url,
        };

        await this.mediator.Send(request, context.CancellationToken);
    }
}
