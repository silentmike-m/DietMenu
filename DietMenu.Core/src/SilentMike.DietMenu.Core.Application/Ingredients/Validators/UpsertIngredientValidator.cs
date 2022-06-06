namespace SilentMike.DietMenu.Core.Application.Ingredients.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;

internal sealed class UpsertIngredientValidator : AbstractValidator<UpsertIngredient>
{
    public UpsertIngredientValidator()
    {
        RuleFor(pt => pt.Ingredient)
            .Must(i => i.Exchanger is null or > 0)
            .WithErrorCode(ValidationErrorCodes.UPSERT_INGREDIENT_INVALID_EXCHANGER)
            .WithMessage(ValidationErrorCodes.UPSERT_INGREDIENT_INVALID_EXCHANGER_MESSAGE)
            ;

        RuleFor(pt => pt.Ingredient)
            .Must(pt => pt.Name is null || !string.IsNullOrWhiteSpace(pt.Name))
            .WithErrorCode(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME)
            .WithMessage(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME_MESSAGE)
            ;

        RuleFor(pt => pt.Ingredient)
            .Must(pt => pt.UnitSymbol is null || !string.IsNullOrWhiteSpace(pt.UnitSymbol))
            .WithErrorCode(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT)
            .WithMessage(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT_MESSAGE)
            ;
    }
}
