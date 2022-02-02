namespace SilentMike.DietMenu.Core.UnitTests.Ingredients;

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Application.Ingredients.Validators;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels.ValueModels;

[TestClass]
public sealed class UpsertIngredientsValidatorTests
{
    [TestMethod]
    public void ShouldPassValidationWHenAllParametersAreNull()
    {
        //GIVEN
        var command = new UpsertIngredients
        {
            Ingredients = new List<IngredientToUpsert>
            {
                new()
                {
                    Exchanger = null,
                    Name = null,
                    UnitSymbol = null,
                },
            },
        };

        var validator = new UpsertIngredientsValidator();

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
        var command = new UpsertIngredients
        {
            Ingredients = new List<IngredientToUpsert>
            {
                new()
                {
                    Exchanger = 0,
                    Name = "",
                    UnitSymbol = "",
                },
            },
        };

        var validator = new UpsertIngredientsValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(3)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_NAME
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_NAME_MESSAGE)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_UNIT
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_UNIT_MESSAGE)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_INGREDIENTS_INVALID_EXCHANGER
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_INGREDIENTS_INVALID_EXCHANGER_MESSAGE)
            ;
        result.IsValid.Should()
            .BeFalse();
    }

    [TestMethod]
    public void ShouldFailValidationWhenAllParametersAreSpaces()
    {
        //GIVEN
        var command = new UpsertIngredients
        {
            Ingredients = new List<IngredientToUpsert>
            {
                new()
                {
                    Exchanger = 0.5m,
                    Name = " ",
                    UnitSymbol = " ",
                },
            },
        };

        var validator = new UpsertIngredientsValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(2)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_NAME
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_NAME_MESSAGE)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_UNIT
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_UNIT_MESSAGE)
            ;
        result.IsValid.Should()
            .BeFalse();
    }
}
