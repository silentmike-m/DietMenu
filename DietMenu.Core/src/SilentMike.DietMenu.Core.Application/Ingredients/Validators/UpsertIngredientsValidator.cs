namespace SilentMike.DietMenu.Core.Application.Ingredients.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;

internal sealed class UpsertIngredientsValidator : AbstractValidator<UpsertIngredients>
{
    public UpsertIngredientsValidator()
    {
        this.RuleForEach(i => i.Ingredients)
            .SetValidator(new IngredientToUpsertValidator());
    }
}
