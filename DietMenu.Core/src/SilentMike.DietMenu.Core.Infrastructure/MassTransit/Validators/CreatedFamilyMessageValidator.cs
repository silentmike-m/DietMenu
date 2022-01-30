namespace SilentMike.DietMenu.Core.Infrastructure.MassTransit.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Shared.MassTransit;

internal sealed class CreatedFamilyMessageValidator : AbstractValidator<ICreatedFamilyMessage>
{
    public CreatedFamilyMessageValidator()
    {
        RuleFor(i => i.Name)
            .NotEmpty()
            .EmailAddress()
            .WithErrorCode(ValidationErrorCodes.CREATE_FAMILY_MISSING_NAME)
            .WithMessage(ValidationErrorCodes.CREATE_FAMILY_MISSING_NAME_MESSAGE)
            ;
    }
}
