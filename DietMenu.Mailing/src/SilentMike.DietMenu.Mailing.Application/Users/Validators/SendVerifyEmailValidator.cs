namespace SilentMike.DietMenu.Mailing.Application.Users.Validators;

using FluentValidation;
using SilentMike.DietMenu.Mailing.Application.Common.Constants;
using SilentMike.DietMenu.Mailing.Application.Users.Commands;

public sealed class SendVerifyEmailValidator : AbstractValidator<SendVerifyEmail>
{
    public SendVerifyEmailValidator()
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
