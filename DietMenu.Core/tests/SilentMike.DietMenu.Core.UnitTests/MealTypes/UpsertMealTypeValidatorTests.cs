namespace SilentMike.DietMenu.Core.UnitTests.MealTypes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.MealTypes.Commands;
using SilentMike.DietMenu.Core.Application.MealTypes.Validators;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels.ValueModels;

[TestClass]
public sealed class UpsertMealTypeValidatorTests
{
    [TestMethod]
    public void ShouldPassValidationWhenAllParametersAreNull()
    {
        //GIVEN
        var command = new UpsertMealType
        {
            MealType = new MealTypeToUpsert
            {
                Name = null,
                Order = null,
            },
        };

        var validator = new UpsertMealTypeValidator();

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
    public void ShouldFailValidationWhenNameIsSpacesAndOrderIsLessThan1()
    {
        //GIVEN
        var command = new UpsertMealType
        {
            MealType = new MealTypeToUpsert
            {
                Name = "   ",
                Order = 0,
            },
        };

        var validator = new UpsertMealTypeValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(2)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_MEAL_TYPE_EMPTY_NAME
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_MEAL_TYPE_EMPTY_NAME_MESSAGE)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_MEAL_TYPE_INVALID_ORDER
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_MEAL_TYPE_INVALID_ORDER_MESSAGE)
            ;
    }
}
