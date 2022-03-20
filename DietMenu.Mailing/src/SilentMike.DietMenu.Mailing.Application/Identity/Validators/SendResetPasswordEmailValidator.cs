namespace SilentMike.DietMenu.Mailing.Application.Identity.Validators;

using FluentValidation;
using SilentMike.DietMenu.Mailing.Application.Common.Constants;
using SilentMike.DietMenu.Mailing.Application.Identity.Commands;

public sealed class SendResetPasswordEmailValidator : AbstractValidator<SendResetPasswordEmail>
{
    public SendResetPasswordEmailValidator()
    {
        RuleFor(i => i.Email)
            .NotEmpty()
            .EmailAddress()
            .WithErrorCode(ValidationErrorCodes.SEND_RESET_PASSWORD_INCORRECT_EMAIL_FORMAT)
            .WithMessage(ValidationErrorCodes.SEND_RESET_PASSWORD_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;
    }
}
