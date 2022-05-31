namespace SilentMike.DietMenu.Core.UnitTests.Recipes;

using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Application.Exceptions.MealTypes;
using SilentMike.DietMenu.Core.Application.Recipes.CommandHandlers;
using SilentMike.DietMenu.Core.Application.Recipes.Commands;
using SilentMike.DietMenu.Core.Application.Recipes.ValueModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class UpsertRecipesHandlerTests : IDisposable
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly Guid ingredientId = Guid.NewGuid();
    private readonly Guid mealTypeId = Guid.NewGuid();
    private readonly Guid recipeId = Guid.NewGuid();
    private readonly Guid recipeIngredientId = Guid.NewGuid();

    private readonly DietMenuDbContextFactory factory;
    private readonly FamilyRepository familyRepository;
    private readonly IngredientRepository ingredientRepository;
    private readonly NullLogger<UpsertRecipeHandler> logger;
    private readonly MealTypeRepository mealTypeRepository;
    private readonly RecipeRepository recipeRepository;

    public UpsertRecipesHandlerTests()
    {
        var family = new FamilyEntity(this.familyId);

        var ingredientType = new IngredientTypeEntity(Guid.NewGuid())
        {
            FamilyId = this.familyId,
        };

        var ingredient = new IngredientEntity(this.ingredientId)
        {
            FamilyId = this.familyId,
            TypeId = ingredientType.Id,
        };

        var mealType = new MealTypeEntity(this.mealTypeId)
        {
            FamilyId = this.familyId,
        };

        var recipe = new RecipeEntity(this.recipeId)
        {
            FamilyId = this.familyId,
            Ingredients = new List<RecipeIngredientEntity>
            {
                new (this.recipeIngredientId)
                {
                    IngredientId = this.ingredientId,
                    Quantity = 1,
                    RecipeId = this.recipeId,
                },
            },
            MealTypeId = this.mealTypeId,
        };

        this.factory = new DietMenuDbContextFactory(family, ingredientType, ingredient, mealType, recipe);

        this.familyRepository = new FamilyRepository(this.factory.Context);
        this.ingredientRepository = new IngredientRepository(this.factory.Context);
        this.logger = new NullLogger<UpsertRecipeHandler>();
        this.mealTypeRepository = new MealTypeRepository(this.factory.Context);
        this.recipeRepository = new RecipeRepository(this.factory.Context);
    }

    [TestMethod]
    public async Task ShouldThrowFamilyNotFoundWhenInvalidIdOnUpsertRecipes()
    {
        //GIVEN
        var command = new UpsertRecipe
        {
            FamilyId = Guid.NewGuid(),
        };

        var commandHandler = new UpsertRecipeHandler(this.familyRepository, this.ingredientRepository, this.logger, this.mealTypeRepository, this.recipeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyNotFoundException>()
                .Where(i =>
                    i.Code == ErrorCodes.FAMILY_NOT_FOUND
                    && i.Id == command.FamilyId)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowValidationExceptionIfNameIsSpacesOnCreateOnUpsertRecipes()
    {
        //GIVEN
        var command = new UpsertRecipe
        {
            FamilyId = this.familyId,
            Recipe = new RecipeToUpsert
            {
                Carbohydrates = 1,
                Energy = 1,
                Fat = 1,
                Id = Guid.NewGuid(),
                Name = "  ",
                Protein = 1,
            },
        };

        var commandHandler = new UpsertRecipeHandler(this.familyRepository, this.ingredientRepository, this.logger, this.mealTypeRepository, this.recipeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i => i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_RECIPE_EMPTY_NAME)
                            && i.Errors[ValidationErrorCodes.UPSERT_RECIPE_EMPTY_NAME]
                                .Contains(ValidationErrorCodes.UPSERT_RECIPE_EMPTY_NAME_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldThrowValidationExceptionIfNameIsNullAndParametersAreNullOnCreateOnUpsertRecipes()
    {
        //GIVEN
        var command = new UpsertRecipe
        {
            FamilyId = this.familyId,
            Recipe = new RecipeToUpsert
            {
                Carbohydrates = null,
                Energy = null,
                Fat = null,
                Id = Guid.NewGuid(),
                MealTypeId = null,
                Name = null,
                Protein = null,
            },
        };

        var commandHandler = new UpsertRecipeHandler(this.familyRepository, this.ingredientRepository, this.logger, this.mealTypeRepository, this.recipeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i => i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_RECIPE_EMPTY_NAME)
                            && i.Errors[ValidationErrorCodes.UPSERT_RECIPE_EMPTY_NAME]
                                .Contains(ValidationErrorCodes.UPSERT_RECIPE_EMPTY_NAME_MESSAGE))
                .Where(i => i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_RECIPE_INVALID_CARBOHYDRATES)
                            && i.Errors[ValidationErrorCodes.UPSERT_RECIPE_INVALID_CARBOHYDRATES]
                                .Contains(ValidationErrorCodes.UPSERT_RECIPE_INVALID_CARBOHYDRATES_MESSAGE))
                .Where(i => i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_RECIPE_INVALID_ENERGY)
                            && i.Errors[ValidationErrorCodes.UPSERT_RECIPE_INVALID_ENERGY]
                                .Contains(ValidationErrorCodes.UPSERT_RECIPE_INVALID_ENERGY_MESSAGE))
                .Where(i => i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_RECIPE_INVALID_FAT)
                            && i.Errors[ValidationErrorCodes.UPSERT_RECIPE_INVALID_FAT]
                                .Contains(ValidationErrorCodes.UPSERT_RECIPE_INVALID_FAT_MESSAGE))
                .Where(i => i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_RECIPE_EMPTY_MEAL_TYPE)
                            && i.Errors[ValidationErrorCodes.UPSERT_RECIPE_EMPTY_MEAL_TYPE]
                                .Contains(ValidationErrorCodes.UPSERT_RECIPE_EMPTY_MEAL_TYPE_MESSAGE))
                .Where(i => i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_RECIPE_INVALID_PROTEIN)
                            && i.Errors[ValidationErrorCodes.UPSERT_RECIPE_INVALID_PROTEIN]
                                .Contains(ValidationErrorCodes.UPSERT_RECIPE_INVALID_PROTEIN_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldThrowMealTypeNotFoundWhenInvalidIdOnCreateOnUpsertRecipes()
    {
        //GIVEN
        var recipeToUpsert = new RecipeToUpsert
        {
            Carbohydrates = 1,
            Energy = 1,
            Fat = 1,
            Id = Guid.NewGuid(),
            MealTypeId = Guid.NewGuid(),
            Name = "new recipe",
            Protein = 1,
        };

        var command = new UpsertRecipe
        {
            FamilyId = this.familyId,
            Recipe = recipeToUpsert,
        };

        var commandHandler = new UpsertRecipeHandler(this.familyRepository, this.ingredientRepository, this.logger, this.mealTypeRepository, this.recipeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<MealTypeNotFoundException>()
                .Where(i =>
                    i.Code == ErrorCodes.MEAL_TYPE_NOT_FOUND
                    && i.Id == recipeToUpsert.MealTypeId)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowMealTypeNotFoundWhenInvalidIdOnUpdateOnUpsertRecipes()
    {
        //GIVEN
        var recipeToUpsert = new RecipeToUpsert
        {
            Carbohydrates = 1,
            Energy = 1,
            Fat = 1,
            Id = this.recipeId,
            MealTypeId = Guid.NewGuid(),
            Name = "new recipe",
            Protein = 1,
        };

        var command = new UpsertRecipe
        {
            FamilyId = this.familyId,
            Recipe = recipeToUpsert,
        };

        var commandHandler = new UpsertRecipeHandler(this.familyRepository, this.ingredientRepository, this.logger, this.mealTypeRepository, this.recipeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<MealTypeNotFoundException>()
                .Where(i =>
                    i.Code == ErrorCodes.MEAL_TYPE_NOT_FOUND
                    && i.Id == recipeToUpsert.MealTypeId)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowIngredientNotFoundWhenInvalidIdOnCreateOnUpsertRecipes()
    {
        //GIVEN
        var ingredientToUpsert = new RecipeIngredientToUpsert
        {
            Id = Guid.NewGuid(),
            IngredientId = Guid.NewGuid(),
            Quantity = 23,
        };

        var recipeToUpsert = new RecipeToUpsert
        {
            Carbohydrates = 1,
            Energy = 1,
            Fat = 1,
            Id = Guid.NewGuid(),
            Ingredients = new List<RecipeIngredientToUpsert>
            {
                ingredientToUpsert,
            },
            MealTypeId = this.mealTypeId,
            Name = "new recipe",
            Protein = 1,
        };

        var command = new UpsertRecipe
        {
            FamilyId = this.familyId,
            Recipe = recipeToUpsert,
        };

        var commandHandler = new UpsertRecipeHandler(this.familyRepository, this.ingredientRepository, this.logger, this.mealTypeRepository, this.recipeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<IngredientNotFoundException>()
                .Where(i =>
                    i.Code == ErrorCodes.INGREDIENT_NOT_FOUND
                    && i.Id == ingredientToUpsert.IngredientId)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowIngredientNotFoundWhenInvalidIdOnUpdateOnUpsertRecipes()
    {
        //GIVEN
        var ingredientToUpsert = new RecipeIngredientToUpsert
        {
            Id = Guid.NewGuid(),
            IngredientId = Guid.NewGuid(),
            Quantity = 23,
        };

        var recipeToUpsert = new RecipeToUpsert
        {
            Carbohydrates = 1,
            Energy = 1,
            Fat = 1,
            Id = this.recipeId,
            Ingredients = new List<RecipeIngredientToUpsert>
            {
                ingredientToUpsert,
            },
            MealTypeId = this.mealTypeId,
            Name = "new recipe",
            Protein = 1,
        };

        var command = new UpsertRecipe
        {
            FamilyId = this.familyId,
            Recipe = recipeToUpsert,
        };

        var commandHandler = new UpsertRecipeHandler(this.familyRepository, this.ingredientRepository, this.logger, this.mealTypeRepository, this.recipeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<IngredientNotFoundException>()
                .Where(i =>
                    i.Code == ErrorCodes.INGREDIENT_NOT_FOUND
                    && i.Id == ingredientToUpsert.IngredientId)
            ;
    }

    [TestMethod]
    public async Task ShouldCreateRecipeOnUpsertRecipes()
    {
        //GIVEN
        var ingredientToUpsert = new RecipeIngredientToUpsert
        {
            Id = Guid.NewGuid(),
            IngredientId = this.ingredientId,
            Quantity = 23,
        };

        var recipeToUpsert = new RecipeToUpsert
        {
            Carbohydrates = 1,
            Description = "new recipe description",
            Energy = 1,
            Fat = 1,
            Id = Guid.NewGuid(),
            Ingredients = new List<RecipeIngredientToUpsert>
            {
                ingredientToUpsert,
            },
            MealTypeId = this.mealTypeId,
            Name = "new recipe",
            Protein = 1,
        };

        var command = new UpsertRecipe
        {
            FamilyId = this.familyId,
            Recipe = recipeToUpsert,
        };

        var commandHandler = new UpsertRecipeHandler(this.familyRepository, this.ingredientRepository, this.logger, this.mealTypeRepository, this.recipeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        var recipe = this.recipeRepository.Get(command.FamilyId, recipeToUpsert.Id);

        recipe.Should()
            .NotBeNull()
            ;
        recipe!.Carbohydrates.Should()
            .Be(recipeToUpsert.Carbohydrates)
            ;
        recipe.Description.Should()
            .Be(recipeToUpsert.Description)
            ;
        recipe.Energy.Should()
            .Be(recipeToUpsert.Energy)
            ;
        recipe.FamilyId.Should()
            .Be(command.FamilyId)
            ;
        recipe.Fat.Should()
            .Be(recipeToUpsert.Fat)
            ;
        recipe.MealTypeId.Should()
            .Be(recipeToUpsert.MealTypeId.Value)
            ;
        recipe.Name.Should()
            .Be(recipeToUpsert.Name)
            ;
        recipe.Protein.Should()
            .Be(recipeToUpsert.Protein)
            ;

        var recipeIngredients = this.factory.Context.RecipeIngredients.Where(i => i.RecipeId == recipeToUpsert.Id);
        recipeIngredients.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Id == ingredientToUpsert.Id
                && i.IngredientId == ingredientToUpsert.IngredientId
                && i.Quantity == ingredientToUpsert.Quantity
                && i.RecipeId == recipeToUpsert.Id)
            ;
    }

    [TestMethod]
    public async Task ShouldUpdateRecipe()
    {
        //GIVEN
        var ingredientToUpsert = new RecipeIngredientToUpsert
        {
            Id = Guid.NewGuid(),
            IngredientId = this.ingredientId,
            Quantity = 23,
        };

        var recipeToUpsert = new RecipeToUpsert
        {
            Carbohydrates = 1,
            Description = "new recipe description",
            Energy = 1,
            Fat = 1,
            Id = this.recipeId,
            Ingredients = new List<RecipeIngredientToUpsert>
            {
                ingredientToUpsert,
            },
            MealTypeId = this.mealTypeId,
            Name = "new recipe",
            Protein = 1,
        };

        var command = new UpsertRecipe
        {
            FamilyId = this.familyId,
            Recipe = recipeToUpsert,
        };

        var commandHandler = new UpsertRecipeHandler(this.familyRepository, this.ingredientRepository, this.logger, this.mealTypeRepository, this.recipeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        var recipe = this.recipeRepository.Get(command.FamilyId, recipeToUpsert.Id);

        recipe.Should()
            .NotBeNull()
            ;
        recipe!.Carbohydrates.Should()
            .Be(recipeToUpsert.Carbohydrates)
            ;
        recipe.Description.Should()
            .Be(recipeToUpsert.Description)
            ;
        recipe.Energy.Should()
            .Be(recipeToUpsert.Energy)
            ;
        recipe.FamilyId.Should()
            .Be(command.FamilyId)
            ;
        recipe.Fat.Should()
            .Be(recipeToUpsert.Fat)
            ;
        recipe.MealTypeId.Should()
            .Be(recipeToUpsert.MealTypeId.Value)
            ;
        recipe.Name.Should()
            .Be(recipeToUpsert.Name)
            ;
        recipe.Protein.Should()
            .Be(recipeToUpsert.Protein)
            ;

        var recipeIngredients = this.factory.Context.RecipeIngredients.Where(i => i.RecipeId == recipeToUpsert.Id);
        recipeIngredients.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Id == ingredientToUpsert.Id
                && i.IngredientId == ingredientToUpsert.IngredientId
                && i.Quantity == ingredientToUpsert.Quantity
                && i.RecipeId == recipeToUpsert.Id)
            ;
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }
}
