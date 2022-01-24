namespace SilentMike.DietMenu.Core.Application.Auth.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Auth.Commands;
using SilentMike.DietMenu.Core.Application.Common.Constants;

internal sealed class CreateUserValidator : AbstractValidator<CreateUser>
{
    public CreateUserValidator()
    {
        this.RuleFor(r => r.CreateCode)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_EMPTY_CREATE_CODE)
            .WithMessage(ValidationErrorCodes.CREATE_USER_EMPTY_CREATE_CODE_MESSAGE)
            ;

        this.RuleFor(r => r.User.Id)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_EMPTY_ID)
            .WithMessage(ValidationErrorCodes.CREATE_USER_EMPTY_ID_MESSAGE)
            ;

        this.RuleFor(r => r.User.Email)
            .EmailAddress()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_INCORRECT_EMAIL_FORMAT)
            .WithMessage(ValidationErrorCodes.CREATE_USER_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        this.RuleFor(r => r.User.FamilyName)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_EMPTY_FAMILY_NAME)
            .WithMessage(ValidationErrorCodes.CREATE_USER_EMPTY_FAMILY_NAME_MESSAGE)
            ;

        this.RuleFor(r => r.User.FirstName)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_EMPTY_FIRST_NAME)
            .WithMessage(ValidationErrorCodes.CREATE_USER_EMPTY_FIRST_NAME_MESSAGE)
            ;

        this.RuleFor(r => r.User.Password)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_EMPTY_PASSWORD)
            .WithMessage(ValidationErrorCodes.CREATE_USER_EMPTY_PASSWORD_MESSAGE)
            ;

        this.RuleFor(r => r.User.UserName)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_EMPTY_USER_NAME)
            .WithMessage(ValidationErrorCodes.CREATE_USER_EMPTY_USER_NAME_MESSAGE)
            ;
    }
}
