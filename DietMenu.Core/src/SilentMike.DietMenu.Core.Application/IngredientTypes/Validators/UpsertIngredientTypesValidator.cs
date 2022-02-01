namespace SilentMike.DietMenu.Core.Application.IngredientTypes.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;

public sealed class UpsertIngredientTypesValidator : AbstractValidator<UpsertIngredientTypes>
{
    public UpsertIngredientTypesValidator()
    {
        this.RuleForEach(i => i.IngredientTypes)
            .SetValidator(new IngredientTypeToUpsertValidator());
    }
}
