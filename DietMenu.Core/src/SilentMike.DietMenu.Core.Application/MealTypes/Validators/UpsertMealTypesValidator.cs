namespace SilentMike.DietMenu.Core.Application.MealTypes.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.MealTypes.Commands;

internal sealed class UpsertMealTypesValidator : AbstractValidator<UpsertMealTypes>
{
    public UpsertMealTypesValidator()
    {
        this.RuleForEach(i => i.MealTypes)
            .SetValidator(new MealTypeToUpsertValidator());
    }
}
