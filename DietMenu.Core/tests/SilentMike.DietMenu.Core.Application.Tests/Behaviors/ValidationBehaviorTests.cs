namespace SilentMike.DietMenu.Core.Application.Tests.Behaviors;

using FluentValidation;
using SilentMike.DietMenu.Core.Application.Common.Behaviors;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Application.Ingredients.Models;
using SilentMike.DietMenu.Core.Application.Ingredients.Validators;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using ErrorCodes = SilentMike.DietMenu.Core.Application.Common.Constants.ErrorCodes;
using ValidationException = SilentMike.DietMenu.Core.Application.Exceptions.ValidationException;

[TestClass]
public sealed class ValidationBehaviorTests
{
    [TestMethod]
    public async Task Should_Not_Exception_When_No_Validation_Errors()
    {
        //GIVEN
        var validators = new List<IValidator<CreateIngredient>>
        {
            new CreateIngredientValidator(),
        };

        var request = new CreateIngredient
        {
            Ingredient = new IngredientToCreate
            {
                Exchanger = 1m,
                Name = "ingredient name",
                Type = IngredientTypeNames.Fruit,
                UnitSymbol = "kg",
            },
        };

        var behavior = new ValidationBehavior<CreateIngredient, Unit>(validators);

        //WHEN
        var action = async () => await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Validation_Exception_When_Are_Validation_Errors()
    {
        //GIVEN
        var validators = new List<IValidator<CreateIngredient>>
        {
            new CreateIngredientValidator(),
        };

        var request = new CreateIngredient
        {
            Ingredient = new IngredientToCreate
            {
                Exchanger = -1,
                Name = "ingredient name",
                Type = "type",
                UnitSymbol = "kg",
            },
        };

        var behavior = new ValidationBehavior<CreateIngredient, Unit>(validators);

        //WHEN
        var action = async () => await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(exception => exception.Code == ErrorCodes.VALIDATION_FAILED)
                .Where(exception => exception.Errors.Count == 2)
            ;
    }
}
