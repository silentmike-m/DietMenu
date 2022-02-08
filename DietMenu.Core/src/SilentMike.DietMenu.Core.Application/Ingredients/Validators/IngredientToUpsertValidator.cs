namespace SilentMike.DietMenu.Core.Application.Ingredients.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels.ValueModels;

internal sealed class IngredientToUpsertValidator : AbstractValidator<IngredientToUpsert>
{
    public IngredientToUpsertValidator()
    {
        RuleFor(pt => pt.Exchanger)
            .Must(i => i is null or > 0)
            .WithErrorCode(ValidationErrorCodes.UPSERT_INGREDIENT_INVALID_EXCHANGER)
            .WithMessage(ValidationErrorCodes.UPSERT_INGREDIENT_INVALID_EXCHANGER_MESSAGE)
            ;

        RuleFor(pt => pt)
            .Must(pt => pt.Name is null || !string.IsNullOrWhiteSpace(pt.Name))
            .WithErrorCode(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME)
            .WithMessage(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME_MESSAGE)
            ;

        RuleFor(pt => pt)
            .Must(pt => pt.UnitSymbol is null || !string.IsNullOrWhiteSpace(pt.UnitSymbol))
            .WithErrorCode(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT)
            .WithMessage(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT_MESSAGE)
            ;
    }
}
