namespace SilentMike.DietMenu.Core.Application.Auth.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Auth.Commands;
using SilentMike.DietMenu.Core.Application.Common.Constants;

internal sealed class ConfirmEmailValidator : AbstractValidator<ConfirmEmail>
{
    public ConfirmEmailValidator()
    {
        this.RuleFor(r => r.Email)
            .EmailAddress()
            .WithErrorCode(ValidationErrorCodes.CONFIRM_EMAIL_EMAIL_FORMAT)
            .WithMessage(ValidationErrorCodes.CONFIRM_EMAIL_EMAIL_FORMAT_MESSAGE)
            ;

        this.RuleFor(r => r.Token)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CONFIRM_EMAIL_EMPTY_TOKEN)
            .WithMessage(ValidationErrorCodes.CONFIRM_EMAIL_EMPTY_TOKEN_MESSAGE)
            ;
    }
}
