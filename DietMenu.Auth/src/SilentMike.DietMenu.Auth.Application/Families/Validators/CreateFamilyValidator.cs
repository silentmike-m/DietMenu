namespace SilentMike.DietMenu.Auth.Application.Families.Validators;

using FluentValidation;
using SilentMike.DietMenu.Auth.Application.Families.Commands;

internal sealed class CreateFamilyValidator : AbstractValidator<CreateFamily>
{
    public CreateFamilyValidator()
    {
        this.RuleFor(request => request.Email)
            .EmailAddress()
            .WithErrorCode(ValidationErrorCodes.CREATE_FAMILY_EMAIL_INVALID_FORMAT)
            .WithMessage(ValidationErrorCodes.CREATE_FAMILY_EMAIL_INVALID_FORMAT_MESSAGE);

        this.RuleFor(request => request.Name)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_FAMILY_EMPTY_NAME)
            .WithMessage(ValidationErrorCodes.CREATE_FAMILY_EMPTY_NAME_MESSAGE);
    }
}
