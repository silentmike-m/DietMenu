namespace SilentMike.DietMenu.Core.UnitTests.Ingredients;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Application.Ingredients.Validators;
using SilentMike.DietMenu.Core.Application.Ingredients.ValueModels;

[TestClass]
public sealed class UpsertIngredientValidatorTests
{
    [TestMethod]
    public void ShouldPassValidationWHenAllParametersAreNull()
    {
        //GIVEN
        var command = new UpsertIngredient
        {
            Ingredient = new IngredientToUpsert
            {
                Exchanger = null,
                Name = null,
                UnitSymbol = null,
            },
        };

        var validator = new UpsertIngredientValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .BeEmpty()
            ;
        result.IsValid.Should()
            .BeTrue();
    }

    [TestMethod]
    public void ShouldFailValidationErrorWhenAllParametersAreEmptyString()
    {
        //GIVEN
        var command = new UpsertIngredient
        {
            Ingredient = new IngredientToUpsert
            {
                Exchanger = 0,
                Name = "",
                UnitSymbol = "",
            },
        };

        var validator = new UpsertIngredientValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(3)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME_MESSAGE)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT_MESSAGE)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_INGREDIENT_INVALID_EXCHANGER
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_INGREDIENT_INVALID_EXCHANGER_MESSAGE)
            ;
        result.IsValid.Should()
            .BeFalse();
    }

    [TestMethod]
    public void ShouldFailValidationWhenAllParametersAreSpaces()
    {
        //GIVEN
        var command = new UpsertIngredient
        {
            Ingredient = new IngredientToUpsert
            {
                Exchanger = 0.5m,
                Name = " ",
                UnitSymbol = " ",
            },
        };

        var validator = new UpsertIngredientValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(2)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME_MESSAGE)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT_MESSAGE)
            ;
        result.IsValid.Should()
            .BeFalse();
    }
}
