namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Validators;

using FluentValidation;
using SilentMike.DietMenu.Mailing.Application.Common.Constants;
using SilentMike.DietMenu.Shared.MassTransit.Identity;

internal sealed class SendVerifyUserMessageValidator : AbstractValidator<ISendVerifyUserMessageRequest>
{
    public SendVerifyUserMessageValidator()
    {
        RuleFor(i => i.Email)
            .NotEmpty()
            .EmailAddress()
            .WithErrorCode(ValidationErrorCodes.SEND_VERIFY_USER_INCORRECT_EMAIL_FORMAT)
            .WithMessage(ValidationErrorCodes.SEND_VERIFY_USER_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;
    }
}
