namespace SilentMike.DietMenu.Core.UnitTests.EntityFramework;

using AutoMapper;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Recipes.ViewModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.AutoMapper;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class RecipeReadServiceTests : IDisposable
{
    private readonly Guid mealTypeId = Guid.NewGuid();
    private readonly RecipeEntity recipeOne;
    private readonly RecipeEntity recipeTwo;
    private readonly Guid userId = Guid.NewGuid();

    private readonly DietMenuDbContextFactory factory;
    private readonly RecipeReadService service;

    public RecipeReadServiceTests()
    {
        var family = new FamilyEntity(Guid.NewGuid());

        var mealType = new MealTypeEntity(this.mealTypeId)
        {
            FamilyId = family.Id,
        };

        var ingredientType = new IngredientTypeEntity(Guid.NewGuid())
        {
            FamilyId = family.Id,
        };

        var ingredientOne = new IngredientEntity(Guid.NewGuid())
        {
            Exchanger = 15.3m,
            FamilyId = family.Id,
            InternalName = "ingredient one",
            Name = "ingredient one",
            TypeId = ingredientType.Id,
            UnitSymbol = "kg",
        };

        var ingredientTwo = new IngredientEntity(Guid.NewGuid())
        {
            Exchanger = 15.3m,
            FamilyId = family.Id,
            InternalName = "ingredient two",
            Name = "ingredient two",
            TypeId = ingredientType.Id,
            UnitSymbol = "kg",
        };

        var recipeOneId = Guid.NewGuid();
        this.recipeOne = new RecipeEntity(recipeOneId)
        {
            Description = "recipe one",
            Carbohydrates = 2.4m,
            Energy = 22.1m,
            FamilyId = family.Id,
            Fat = 100m,
            Ingredients = new List<RecipeIngredientEntity>
            {
                new(Guid.NewGuid())
                {
                    IngredientId = ingredientOne.Id,
                    Quantity = 12,
                    RecipeId = recipeOneId,
                },
            },
            MealTypeId = mealType.Id,
            Name = "recipe one",
            Protein = 2334,
            UserId = this.userId,
        };

        var recipeTwoId = Guid.NewGuid();
        this.recipeTwo = new RecipeEntity(recipeTwoId)
        {
            Description = "recipe two",
            Carbohydrates = 1.9m,
            Energy = 22.5m,
            FamilyId = family.Id,
            Fat = 101.5m,
            Ingredients = new List<RecipeIngredientEntity>
            {
                new(Guid.NewGuid())
                {
                    IngredientId = ingredientTwo.Id,
                    Quantity = 5,
                    RecipeId = recipeTwoId,
                },
            },
            MealTypeId = mealType.Id,
            Name = "recipe two",
            Protein = 23,
            UserId = this.userId,
        };

        var config = new MapperConfiguration(cfg => cfg.AddProfile<RecipeProfile>());
        var mapper = config.CreateMapper();

        this.factory = new DietMenuDbContextFactory(family, ingredientType, mealType, ingredientOne, ingredientTwo, this.recipeOne, this.recipeTwo);

        this.service = new RecipeReadService(this.factory.Context, mapper);
    }

    [TestMethod]
    public async Task ShouldReturnEmptyRecipesGridWhenInvalidUserId()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsPaged = true,
            PageNumber = 0,
            PageSize = 1,
        };

        //WHEN
        var result = await this.service.GetRecipesGridAsync(request, null, null, Guid.NewGuid());

        //THEN
        result.Count.Should()
            .Be(0)
            ;
        result.Elements.Should()
            .BeEmpty()
            ;
    }

    [TestMethod]
    public async Task ShouldReturnRecipesGridWhenInvalidMealTypeId()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsPaged = true,
            PageNumber = 0,
            PageSize = 1,
        };

        //WHEN
        var result = await this.service.GetRecipesGridAsync(request, null, Guid.NewGuid(), this.userId);

        //THEN
        result.Count.Should()
            .Be(0)
            ;
        result.Elements.Should()
            .BeEmpty()
            ;
    }

    [TestMethod]
    public async Task ShouldReturnPagedRecipesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsPaged = true,
            PageNumber = 0,
            PageSize = 1,
        };

        //WHEN
        var result = await this.service.GetRecipesGridAsync(request, null, null, this.userId);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(1)
            ;

        var recipeOneResult = result.Elements[0];
        this.ValidateRecipe(recipeOneResult, this.recipeOne);
    }

    [TestMethod]
    public async Task ShouldReturnSortedAndPagedRecipesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsDescending = true,
            IsPaged = true,
            PageNumber = 0,
            PageSize = 1,
            OrderBy = "name",
        };

        //WHEN
        var result = await this.service.GetRecipesGridAsync(request, null, null, this.userId);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(1)
            ;

        var recipeOneResult = result.Elements[0];
        this.ValidateRecipe(recipeOneResult, this.recipeTwo);
    }

    [TestMethod]
    public async Task ShouldReturnFilteredAndSortedAndPagedRecipesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            Filter = this.recipeOne.Name,
            IsDescending = true,
            IsPaged = true,
            PageNumber = 0,
            PageSize = 1,
            OrderBy = "name",
        };

        //WHEN
        var result = await this.service.GetRecipesGridAsync(request, null, null, this.userId);

        //THEN
        result.Count.Should()
            .Be(1)
            ;
        result.Elements.Should()
            .HaveCount(1)
            ;

        var recipeOneResult = result.Elements[0];
        this.ValidateRecipe(recipeOneResult, this.recipeOne);
    }

    [TestMethod]
    public async Task ShouldReturnFilteredByIngredientAndSortedAndPagedRecipesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsDescending = true,
            IsPaged = true,
            PageNumber = 0,
            PageSize = 1,
            OrderBy = "name",
        };

        //WHEN
        var result = await this.service.GetRecipesGridAsync(request, "ingredient two", null, this.userId);

        //THEN
        result.Count.Should()
            .Be(1)
            ;
        result.Elements.Should()
            .HaveCount(1)
            ;

        var recipeOneResult = result.Elements[0];
        this.ValidateRecipe(recipeOneResult, this.recipeTwo);
    }

    [TestMethod]
    public async Task ShouldReturnFilteredByMealTypeAndSortedAndPagedRecipesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsDescending = true,
            IsPaged = true,
            PageNumber = 0,
            PageSize = 1,
            OrderBy = "name",
        };

        //WHEN
        var result = await this.service.GetRecipesGridAsync(request, null, this.mealTypeId, this.userId);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(1)
            ;

        var recipeOneResult = result.Elements[0];
        this.ValidateRecipe(recipeOneResult, this.recipeTwo);
    }

    [TestMethod]
    public async Task ShouldReturnNotPagedRecipesGrid()
    {
        //GIVEN
        var request = new GridRequest();

        //WHEN
        var result = await this.service.GetRecipesGridAsync(request, null, null, this.userId);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(2)
            ;

        var recipeOneResult = result.Elements[0];
        var recipe = this.factory.Context.Recipes.Single(i => i.Id == recipeOneResult.Id);
        this.ValidateRecipe(recipeOneResult, recipe);

        var recipeTwoResult = result.Elements[1];
        recipe = this.factory.Context.Recipes.Single(i => i.Id == recipeTwoResult.Id);
        this.ValidateRecipe(recipeTwoResult, recipe);
    }

    [TestMethod]
    public async Task ShouldReturnSortedNotPagedRecipesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsDescending = true,
            OrderBy = "name",
        };

        //WHEN
        var result = await this.service.GetRecipesGridAsync(request, null, null, this.userId);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(2)
            ;

        var recipeOneResult = result.Elements[0];
        ValidateRecipe(recipeOneResult, this.recipeTwo);

        var recipeTwoResult = result.Elements[1];
        ValidateRecipe(recipeTwoResult, this.recipeOne);
    }

    [TestMethod]
    public async Task ShouldReturnFilteredAndSortedAndNotPagedRecipesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            Filter = this.recipeOne.Name,
            IsDescending = false,
            OrderBy = "name",
        };

        //WHEN
        var result = await this.service.GetRecipesGridAsync(request, null, null, this.userId);

        //THEN
        result.Count.Should()
            .Be(1)
            ;
        result.Elements.Should()
            .HaveCount(1)
            ;

        var recipeOneResult = result.Elements[0];
        ValidateRecipe(recipeOneResult, this.recipeOne);
    }

    private void ValidateRecipe(Recipe recipeOneResult, RecipeEntity recipe)
    {
        recipeOneResult.Carbohydrates.Should()
            .Be(recipe.Carbohydrates)
            ;
        recipeOneResult.Description.Should()
            .Be(recipe.Description)
            ;
        recipeOneResult.Energy.Should()
            .Be(recipe.Energy)
            ;
        recipeOneResult.Fat.Should()
            .Be(recipe.Fat)
            ;
        recipeOneResult.MealTypeId.Should()
            .Be(recipe.MealTypeId)
            ;
        recipeOneResult.MealTypeName.Should()
            .Be(recipe.MealType.Name)
            ;
        recipeOneResult.Name.Should()
            .Be(recipe.Name)
            ;
        recipeOneResult.Protein.Should()
            .Be(recipe.Protein)
            ;

        var ingredientResult = recipeOneResult.Ingredients[0];
        var ingredient = this.factory.Context.RecipeIngredients.Single(i => i.Id == ingredientResult.Id);

        ingredientResult.IngredientExchanger.Should()
            .Be(ingredient.Ingredient.Exchanger)
            ;
        ingredientResult.IngredientId.Should()
            .Be(ingredient.IngredientId)
            ;
        ingredientResult.IngredientName.Should()
            .Be(ingredient.Ingredient.Name)
            ;
        ingredientResult.IngredientTypeId.Should()
            .Be(ingredient.Ingredient.TypeId)
            ;
        ingredientResult.IngredientTypeName.Should()
            .Be(ingredient.Ingredient.Type.Name)
            ;
        ingredientResult.Quantity.Should()
            .Be(ingredient.Quantity)
            ;
        ingredientResult.UnitSymbol.Should()
            .Be(ingredient.Ingredient.UnitSymbol)
            ;
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }
}
