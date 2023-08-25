namespace SilentMike.DietMenu.Core.Application.Tests.Ingredients.Validators;

using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Application.Ingredients.Models;
using SilentMike.DietMenu.Core.Application.Ingredients.Validators;
using SilentMike.DietMenu.Core.Domain.Common.Constants;

[TestClass]
public sealed class CreateIngredientValidatorTests
{
    [TestMethod]
    public async Task Should_Not_Pass_Validation_When_Data_Are_Not_Correct()
    {
        //GIVEN
        var ingredientToCreate = new IngredientToCreate
        {
            Exchanger = -1m,
            Name = " ",
            Type = "type",
            UnitSymbol = "kg",
        };

        var request = new CreateIngredient
        {
            Ingredient = ingredientToCreate,
        };

        var validator = new CreateIngredientValidator();

        //WHEN
        var result = await validator.ValidateAsync(request);

        //THEN
        result.Errors.Should()
            .HaveCount(3)
            .And
            .Contain(failure =>
                failure.ErrorCode == ValidationErrorCodes.CREATE_INGREDIENT_INVALID_EXCHANGER && failure.ErrorMessage == ValidationErrorCodes.CREATE_INGREDIENT_INVALID_EXCHANGER_MESSAGE
            )
            .And
            .Contain(failure =>
                failure.ErrorCode == ValidationErrorCodes.CREATE_INGREDIENT_EMPTY_NAME && failure.ErrorMessage == ValidationErrorCodes.CREATE_INGREDIENT_EMPTY_NAME_MESSAGE
            )
            .And
            .Contain(failure =>
                failure.ErrorCode == ValidationErrorCodes.CREATE_INGREDIENT_INVALID_TYPE && failure.ErrorMessage == ValidationErrorCodes.CREATE_INGREDIENT_INVALID_TYPE_MESSAGE
            )
            ;
    }

    [TestMethod]
    public async Task Should_Not_Pass_Validation_When_Exchanger_Is_Less_Than_Zero()
    {
        //GIVEN
        var ingredientToCreate = new IngredientToCreate
        {
            Exchanger = -1m,
            Name = "ingredient name",
            Type = IngredientTypeNames.Fruit,
            UnitSymbol = "kg",
        };

        var request = new CreateIngredient
        {
            Ingredient = ingredientToCreate,
        };

        var validator = new CreateIngredientValidator();

        //WHEN
        var result = await validator.ValidateAsync(request);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(failure =>
                failure.ErrorCode == ValidationErrorCodes.CREATE_INGREDIENT_INVALID_EXCHANGER && failure.ErrorMessage == ValidationErrorCodes.CREATE_INGREDIENT_INVALID_EXCHANGER_MESSAGE
            )
            ;
    }

    [TestMethod]
    public async Task Should_Not_Pass_Validation_When_Name_Is_Empty()
    {
        //GIVEN
        var ingredientToCreate = new IngredientToCreate
        {
            Exchanger = 1m,
            Name = "",
            Type = IngredientTypeNames.Fruit,
            UnitSymbol = "kg",
        };

        var request = new CreateIngredient
        {
            Ingredient = ingredientToCreate,
        };

        var validator = new CreateIngredientValidator();

        //WHEN
        var result = await validator.ValidateAsync(request);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(failure =>
                failure.ErrorCode == ValidationErrorCodes.CREATE_INGREDIENT_EMPTY_NAME && failure.ErrorMessage == ValidationErrorCodes.CREATE_INGREDIENT_EMPTY_NAME_MESSAGE
            )
            ;
    }

    [TestMethod]
    public async Task Should_Not_Pass_Validation_When_Name_Is_White_Spaces()
    {
        //GIVEN
        var ingredientToCreate = new IngredientToCreate
        {
            Exchanger = 1m,
            Name = "    ",
            Type = IngredientTypeNames.Fruit,
            UnitSymbol = "kg",
        };

        var request = new CreateIngredient
        {
            Ingredient = ingredientToCreate,
        };

        var validator = new CreateIngredientValidator();

        //WHEN
        var result = await validator.ValidateAsync(request);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(failure =>
                failure.ErrorCode == ValidationErrorCodes.CREATE_INGREDIENT_EMPTY_NAME && failure.ErrorMessage == ValidationErrorCodes.CREATE_INGREDIENT_EMPTY_NAME_MESSAGE
            )
            ;
    }

    [DataRow(0), DataRow(1), DataTestMethod]
    public async Task Should_Pass_Validation_When_All_Data_Are_Correct(int exchanger)
    {
        //GIVEN
        var ingredientToCreate = new IngredientToCreate
        {
            Exchanger = exchanger,
            Name = "ingredient name",
            Type = IngredientTypeNames.Fruit,
            UnitSymbol = "kg",
        };

        var request = new CreateIngredient
        {
            Ingredient = ingredientToCreate,
        };

        var validator = new CreateIngredientValidator();

        //WHEN
        var result = await validator.ValidateAsync(request);

        //THEN
        result.Errors.Should()
            .BeEmpty()
            ;

        result.IsValid.Should()
            .BeTrue()
            ;
    }
}
