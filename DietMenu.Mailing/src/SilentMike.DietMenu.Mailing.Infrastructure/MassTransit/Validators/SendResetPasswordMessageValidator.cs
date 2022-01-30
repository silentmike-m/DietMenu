namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Validators;

using FluentValidation;
using SilentMike.DietMenu.Mailing.Application.Common.Constants;
using SilentMike.DietMenu.Shared.MassTransit;

internal sealed class SendResetPasswordMessageValidator : AbstractValidator<ISendResetPasswordMessage>
{
    public SendResetPasswordMessageValidator()
    {
        RuleFor(i => i.Email)
            .NotEmpty()
            .EmailAddress()
            .WithErrorCode(ValidationErrorCodes.SEND_RESET_PASSWORD_INCORRECT_EMAIL_FORMAT)
            .WithMessage(ValidationErrorCodes.SEND_RESET_PASSWORD_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;
    }
}
