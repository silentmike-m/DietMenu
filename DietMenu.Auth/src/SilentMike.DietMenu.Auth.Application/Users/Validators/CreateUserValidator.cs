namespace SilentMike.DietMenu.Auth.Application.Users.Validators;

using FluentValidation;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Users.Commands;

internal sealed class CreateUserValidator : AbstractValidator<CreateUser>
{
    public CreateUserValidator()
    {
        this.RuleFor(i => i.Email)
            .NotEmpty()
            .EmailAddress()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_INCORRECT_EMAIL_FORMAT)
            .WithMessage(ValidationErrorCodes.CREATE_USER_INCORRECT_EMAIL_FORMAT_MESSAGE);

        this.RuleFor(i => i.Family)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_MISSING_FAMILY)
            .WithMessage(ValidationErrorCodes.CREATE_USER_MISSING_FAMILY_MESSAGE);

        this.RuleFor(i => i.FirstName)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_MISSING_FIRST_NAME)
            .WithMessage(ValidationErrorCodes.CREATE_USER_MISSING_FIRST_NAME_MESSAGE);

        this.RuleFor(i => i.Password)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_MISSING_PASSWORD)
            .WithMessage(ValidationErrorCodes.CREATE_USER_MISSING_PASSWORD_MESSAGE);

        this.RuleFor(i => i.RegisterCode)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_MISSING_REGISTER_CODE)
            .WithMessage(ValidationErrorCodes.CREATE_USER_MISSING_REGISTER_CODE_MESSAGE);
    }
}
