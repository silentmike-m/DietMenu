namespace SilentMike.DietMenu.Core.Application.Ingredients.Validators;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Extensions;

public sealed class CreateIngredientValidator : AbstractValidator<CreateIngredient>
{
    public CreateIngredientValidator()
    {
        this.RuleFor(request => request.Ingredient.Exchanger)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(ValidationErrorCodes.CREATE_INGREDIENT_INVALID_EXCHANGER)
            .WithMessage(ValidationErrorCodes.CREATE_INGREDIENT_INVALID_EXCHANGER_MESSAGE)
            ;

        this.RuleFor(request => request.Ingredient.Name)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.CREATE_INGREDIENT_EMPTY_NAME)
            .WithMessage(ValidationErrorCodes.CREATE_INGREDIENT_EMPTY_NAME_MESSAGE)
            ;

        this.RuleFor(request => request.Ingredient.Type)
            .Must(BeProperType)
            .WithErrorCode(ValidationErrorCodes.CREATE_INGREDIENT_INVALID_TYPE)
            .WithMessage(ValidationErrorCodes.CREATE_INGREDIENT_INVALID_TYPE_MESSAGE)
            ;
    }

    private static bool BeProperType(string type)
    {
        var typeName = IngredientTypeNames.IngredientTypes.GetIgnoreCase(type);

        return typeName is not null;
    }
}
