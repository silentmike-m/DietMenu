namespace SilentMike.DietMenu.Core.Application.MealTypes.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.MealTypes.Commands;

internal sealed class UpsertMealTypeValidator : AbstractValidator<UpsertMealType>
{
    public UpsertMealTypeValidator()
    {
        this.RuleFor(i => i.MealType.Name)
            .Must(s => s is null || !string.IsNullOrWhiteSpace(s))
            .WithErrorCode(ValidationErrorCodes.UPSERT_MEAL_TYPE_EMPTY_NAME)
            .WithMessage(ValidationErrorCodes.UPSERT_MEAL_TYPE_EMPTY_NAME_MESSAGE);

        this.RuleFor(i => i.MealType.Order)
            .Must(s => s is null or >= 1)
            .WithErrorCode(ValidationErrorCodes.UPSERT_MEAL_TYPE_INVALID_ORDER)
            .WithMessage(ValidationErrorCodes.UPSERT_MEAL_TYPE_INVALID_ORDER_MESSAGE);
    }
}
