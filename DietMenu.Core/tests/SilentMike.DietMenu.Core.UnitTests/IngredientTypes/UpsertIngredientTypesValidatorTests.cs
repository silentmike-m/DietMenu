namespace SilentMike.DietMenu.Core.UnitTests.IngredientTypes;

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Validators;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels.ValueModels;

[TestClass]
public sealed class UpsertIngredientTypesValidatorTests
{
    [TestMethod]
    public void ShouldPassValidationWhenAllParametersAreEmpty()
    {
        //GIVEN
        var command = new UpsertIngredientTypes
        {
            IngredientTypes = new List<IngredientTypeToUpsert>
            {
                new()
                {
                    Name = null,
                },
            },
        };

        var validator = new UpsertIngredientTypesValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .BeEmpty()
            ;
        result.IsValid.Should()
            .BeTrue()
            ;
    }

    [TestMethod]
    public void ShouldFailValidationWhenNameIsSpaces()
    {
        //GIVEN
        var command = new UpsertIngredientTypes
        {
            IngredientTypes = new List<IngredientTypeToUpsert>
            {
                new()
                {
                    Name = "   ",
                },
            },
        };

        var validator = new UpsertIngredientTypesValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_INGREDIENT_TYPES_EMPTY_NAME
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_INGREDIENT_TYPES_EMPTY_NAME_MESSAGE)
            ;
    }
}
