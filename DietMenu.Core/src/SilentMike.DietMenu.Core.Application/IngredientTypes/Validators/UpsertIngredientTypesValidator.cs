namespace SilentMike.DietMenu.Core.Application.IngredientTypes.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;

internal sealed class UpsertIngredientTypesValidator : AbstractValidator<UpsertIngredientType>
{
    public UpsertIngredientTypesValidator()
    {
        this.RuleFor(i => i.IngredientType.Name)
            .Must(s => s is null || !string.IsNullOrWhiteSpace(s))
            .WithErrorCode(ValidationErrorCodes.UPSERT_INGREDIENT_TYPE_EMPTY_NAME)
            .WithMessage(ValidationErrorCodes.UPSERT_INGREDIENT_TYPE_EMPTY_NAME_MESSAGE);
    }
}
