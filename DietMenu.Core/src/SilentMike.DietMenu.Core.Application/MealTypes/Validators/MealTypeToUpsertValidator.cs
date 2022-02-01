namespace SilentMike.DietMenu.Core.Application.MealTypes.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels.ValueModels;

internal sealed class MealTypeToUpsertValidator : AbstractValidator<MealTypeToUpsert>
{
    public MealTypeToUpsertValidator()
    {
        this.RuleFor(i => i.Name)
            .Must(s => s is null || !string.IsNullOrWhiteSpace(s))
            .WithErrorCode(ValidationErrorCodes.UPSERT_MEAL_TYPES_EMPTY_NAME)
            .WithMessage(ValidationErrorCodes.UPSERT_MEAL_TYPES_EMPTY_NAME_MESSAGE);

        this.RuleFor(i => i.Order)
            .Must(s => s is null or >= 1)
            .WithErrorCode(ValidationErrorCodes.UPSERT_MEAL_TYPES_INVALID_ORDER)
            .WithMessage(ValidationErrorCodes.UPSERT_MEAL_TYPES_INVALID_ORDER_MESSAGE);
    }
}
