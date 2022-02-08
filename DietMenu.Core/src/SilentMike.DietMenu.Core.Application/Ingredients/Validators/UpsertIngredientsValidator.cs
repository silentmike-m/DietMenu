namespace SilentMike.DietMenu.Core.Application.Ingredients.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;

internal sealed class UpsertIngredientsValidator : AbstractValidator<UpsertIngredient>
{
    public UpsertIngredientsValidator()
    {
        RuleFor(pt => pt.Ingredient.Exchanger)
            .Must(i => i is null or > 0)
            .WithErrorCode(ValidationErrorCodes.UPSERT_INGREDIENT_INVALID_EXCHANGER)
            .WithMessage(ValidationErrorCodes.UPSERT_INGREDIENT_INVALID_EXCHANGER_MESSAGE)
            ;

        RuleFor(pt => pt)
            .Must(pt => pt.Ingredient.Name is null || !string.IsNullOrWhiteSpace(pt.Ingredient.Name))
            .WithErrorCode(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME)
            .WithMessage(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME_MESSAGE)
            ;

        RuleFor(pt => pt)
            .Must(pt => pt.Ingredient.UnitSymbol is null || !string.IsNullOrWhiteSpace(pt.Ingredient.UnitSymbol))
            .WithErrorCode(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT)
            .WithMessage(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT_MESSAGE)
            ;
    }
}
