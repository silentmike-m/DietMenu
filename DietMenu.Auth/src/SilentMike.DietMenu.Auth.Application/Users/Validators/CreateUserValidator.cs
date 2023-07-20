namespace SilentMike.DietMenu.Auth.Application.Users.Validators;

using FluentValidation;
using SilentMike.DietMenu.Auth.Application.Users.Commands;

internal sealed class CreateUserValidator : AbstractValidator<CreateUser>
{
    public CreateUserValidator()
    {
        this.RuleFor(request => request.User.Email)
            .EmailAddress()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_EMAIL_INVALID_FORMAT)
            .WithMessage(ValidationErrorCodes.CREATE_USER_EMAIL_INVALID_FORMAT_MESSAGE)
            ;

        this.RuleFor(request => request.User.FirstName)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_EMPTY_FIRST_NAME)
            .WithMessage(ValidationErrorCodes.CREATE_USER_EMPTY_FIRST_NAME_MESSAGE)
            ;

        this.RuleFor(request => request.User.LastName)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_EMPTY_LAST_NAME)
            .WithMessage(ValidationErrorCodes.CREATE_USER_EMPTY_LAST_NAME_MESSAGE)
            ;

        this.RuleFor(request => request.User.Password)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_USER_EMPTY_PASSWORD)
            .WithMessage(ValidationErrorCodes.CREATE_USER_EMPTY_PASSWORD_MESSAGE)
            ;
    }
}
