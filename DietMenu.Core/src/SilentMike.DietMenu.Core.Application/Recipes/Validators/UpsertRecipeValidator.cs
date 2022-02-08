namespace SilentMike.DietMenu.Core.Application.Recipes.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Recipes.Commands;

internal sealed class UpsertRecipeValidator : AbstractValidator<UpsertRecipe>
{
    public UpsertRecipeValidator()
    {
        this.RuleFor(i => i.Recipe.Carbohydrates)
            .Must(i => i is null or >= 0)
            .WithErrorCode(ValidationErrorCodes.UPSERT_RECIPE_INVALID_CARBOHYDRATES)
            .WithMessage(ValidationErrorCodes.UPSERT_RECIPE_INVALID_CARBOHYDRATES_MESSAGE)
            ;

        this.RuleFor(i => i.Recipe.Energy)
            .Must(i => i is null or >= 0)
            .WithErrorCode(ValidationErrorCodes.UPSERT_RECIPE_INVALID_ENERGY)
            .WithMessage(ValidationErrorCodes.UPSERT_RECIPE_INVALID_ENERGY_MESSAGE)
            ;

        this.RuleFor(i => i.Recipe.Fat)
            .Must(i => i is null or >= 0)
            .WithErrorCode(ValidationErrorCodes.UPSERT_RECIPE_INVALID_FAT)
            .WithMessage(ValidationErrorCodes.UPSERT_RECIPE_INVALID_FAT_MESSAGE)
            ;

        this.RuleFor(i => i.Recipe.Name)
            .Must(i => i is null || !string.IsNullOrWhiteSpace(i))
            .WithErrorCode(ValidationErrorCodes.UPSERT_RECIPE_EMPTY_NAME)
            .WithMessage(ValidationErrorCodes.UPSERT_RECIPE_EMPTY_NAME_MESSAGE)
            ;

        this.RuleFor(i => i.Recipe.Protein)
            .Must(i => i is null or >= 0)
            .WithErrorCode(ValidationErrorCodes.UPSERT_RECIPE_INVALID_PROTEIN)
            .WithMessage(ValidationErrorCodes.UPSERT_RECIPE_INVALID_PROTEIN_MESSAGE)
            ;

        this.RuleForEach(i => i.Recipe.Ingredients)
            .Must(i => i.Quantity > 0)
            .WithErrorCode(ValidationErrorCodes.UPSERT_RECIPE_INGREDIENT_INVALID_QUANTITY)
            .WithMessage(ValidationErrorCodes.UPSERT_RECIPE_INGREDIENT_INVALID_QUANTITY_MESSAGE)
            ;
    }
}
