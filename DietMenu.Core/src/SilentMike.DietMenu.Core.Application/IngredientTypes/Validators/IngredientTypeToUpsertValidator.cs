namespace SilentMike.DietMenu.Core.Application.IngredientTypes.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels.ValueModels;

internal sealed class IngredientTypeToUpsertValidator : AbstractValidator<IngredientTypeToUpsert>
{
    public IngredientTypeToUpsertValidator()
    {
        this.RuleFor(i => i.Name)
            .Must(s => s is null || !string.IsNullOrWhiteSpace(s))
            .WithErrorCode(ValidationErrorCodes.UPSERT_INGREDIENT_TYPES_EMPTY_NAME)
            .WithMessage(ValidationErrorCodes.UPSERT_INGREDIENT_TYPES_EMPTY_NAME_MESSAGE);
    }
}
