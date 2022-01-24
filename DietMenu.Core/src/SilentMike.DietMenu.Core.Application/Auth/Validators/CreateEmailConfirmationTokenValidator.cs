namespace SilentMike.DietMenu.Core.Application.Auth.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Auth.Commands;
using SilentMike.DietMenu.Core.Application.Common.Constants;

internal sealed class CreateEmailConfirmationTokenValidator : AbstractValidator<CreateEmailConfirmationToken>
{
    public CreateEmailConfirmationTokenValidator()
    {
        this.RuleFor(r => r.Email)
            .EmailAddress()
            .WithErrorCode(ValidationErrorCodes.CREATE_EMAIL_CONFIRMATION_TOKEN_INCORRECT_EMAIL_FORMAT)
            .WithMessage(ValidationErrorCodes.CREATE_EMAIL_CONFIRMATION_TOKEN_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;
    }
}
