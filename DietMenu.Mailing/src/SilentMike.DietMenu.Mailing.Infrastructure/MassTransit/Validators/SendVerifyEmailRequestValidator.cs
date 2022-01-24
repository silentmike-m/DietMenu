namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Validators;

using FluentValidation;
using SilentMike.DietMenu.Mailing.Application.Common.Constants;
using SilentMike.DietMenu.Shared.MassTransit;

public sealed class SendVerifyEmailRequestValidator : AbstractValidator<ISendVerifyEmailRequest>
{
    public SendVerifyEmailRequestValidator()
    {
        RuleFor(i => i.Email)
            .NotEmpty()
            .EmailAddress()
            .WithErrorCode(ValidationErrorCodes.SEND_VERIFY_EMAIL_INCORRECT_EMAIL_FORMAT)
            .WithMessage(ValidationErrorCodes.SEND_VERIFY_EMAIL_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        RuleFor(i => i.UserName)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.SEND_VERIFY_EMAIL_EMPTY_USER_NAME)
            .WithMessage(ValidationErrorCodes.SEND_VERIFY_EMAIL_EMPTY_USER_NAME_MESSAGE)
            ;
    }
}
