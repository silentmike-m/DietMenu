namespace SilentMike.DietMenu.Core.UnitTests.Recipes;

using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Recipes.Commands;
using SilentMike.DietMenu.Core.Application.Recipes.Validators;
using SilentMike.DietMenu.Core.Application.Recipes.ViewModels.ValueModels;

[TestClass]
public sealed class UpsertRecipeValidatorTests
{
    [TestMethod]
    public void ShouldPassValidationWhenAllParametersAreNull()
    {
        //GIVEN
        var command = new UpsertRecipe
        {
            Recipe = new RecipeToUpsert
            {
                Carbohydrates = null,
                Description = null,
                Energy = null,
                Fat = null,
                Id = Guid.NewGuid(),
                MealTypeId = null,
                Name = null,
                Protein = null,
            },
        };

        var validator = new UpsertRecipeValidator();

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
    public void ShouldFailValidationWhenAllParametersAreLessThanZero()
    {
        //GIVEN
        var command = new UpsertRecipe
        {
            Recipe = new RecipeToUpsert
            {
                Carbohydrates = -1,
                Description = null,
                Energy = -1,
                Fat = -1,
                Id = Guid.NewGuid(),
                MealTypeId = null,
                Name = null,
                Protein = -1,
            },
        };

        var validator = new UpsertRecipeValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(4)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_RECIPE_INVALID_CARBOHYDRATES
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_RECIPE_INVALID_CARBOHYDRATES_MESSAGE)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_RECIPE_INVALID_ENERGY
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_RECIPE_INVALID_ENERGY_MESSAGE)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_RECIPE_INVALID_FAT
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_RECIPE_INVALID_FAT_MESSAGE)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_RECIPE_INVALID_PROTEIN
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_RECIPE_INVALID_PROTEIN_MESSAGE)
            ;
        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public void ShouldFailValidationWhenNameIsEmptyString()
    {
        //GIVEN
        var command = new UpsertRecipe
        {
            Recipe = new RecipeToUpsert
            {
                Carbohydrates = 1,
                Description = null,
                Energy = 1,
                Fat = 1,
                Id = Guid.NewGuid(),
                MealTypeId = null,
                Name = string.Empty,
                Protein = 1,
            },
        };

        var validator = new UpsertRecipeValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_RECIPE_EMPTY_NAME
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_RECIPE_EMPTY_NAME_MESSAGE)
            ;
        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public void ShouldFailValidationWhenNameIsSpaces()
    {
        //GIVEN
        var command = new UpsertRecipe
        {
            Recipe = new RecipeToUpsert
            {
                Carbohydrates = 1,
                Description = null,
                Energy = 1,
                Fat = 1,
                Id = Guid.NewGuid(),
                MealTypeId = null,
                Name = "  ",
                Protein = 1,
            },
        };

        var validator = new UpsertRecipeValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_RECIPE_EMPTY_NAME
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_RECIPE_EMPTY_NAME_MESSAGE)
            ;
        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public void ShouldFailValidationWhenIngredientQuantityIsLessOrEqualZero()
    {
        //GIVEN
        var command = new UpsertRecipe
        {
            Recipe = new RecipeToUpsert
            {
                Carbohydrates = null,
                Description = null,
                Energy = null,
                Fat = null,
                Id = Guid.NewGuid(),
                Ingredients = new List<RecipeIngredientToUpsert>
                {
                    new()
                    {
                        Quantity = -1,
                    },
                    new()
                    {
                        Quantity = 0,
                    },
                },
                MealTypeId = null,
                Name = null,
                Protein = null,
            },
        };

        var validator = new UpsertRecipeValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(2)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.UPSERT_RECIPE_INGREDIENT_INVALID_QUANTITY
                          && i.ErrorMessage == ValidationErrorCodes.UPSERT_RECIPE_INGREDIENT_INVALID_QUANTITY_MESSAGE)
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }
}
