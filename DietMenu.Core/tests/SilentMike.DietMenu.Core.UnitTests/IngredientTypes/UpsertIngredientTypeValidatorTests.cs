namespace SilentMike.DietMenu.Core.UnitTests.IngredientTypes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Validators;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels.ValueModels;

[TestClass]
public sealed class UpsertIngredientTypeValidatorTests
{
    [TestMethod]
    public void ShouldPassValidationWhenAllParametersAreEmpty()
    {
        //GIVEN
        var command = new UpsertIngredientType
        {
            IngredientType = new IngredientTypeToUpsert
            {
                Name = null,
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
        var command = new UpsertIngredientType
        {
            IngredientType = new IngredientTypeToUpsert
            {
                Name = "   ",
            },
        };

        var validator = new UpsertIngredientTypesValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_INGREDIENT_TYPE_EMPTY_NAME
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_INGREDIENT_TYPE_EMPTY_NAME_MESSAGE)
            ;
    }
}
