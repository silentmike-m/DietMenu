namespace SilentMike.DietMenu.Core.UnitTests.Recipes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions.Recipes;
using SilentMike.DietMenu.Core.Application.Recipes.CommandHandlers;
using SilentMike.DietMenu.Core.Application.Recipes.Commands;
using SilentMike.DietMenu.Core.Application.Recipes.Events;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class DeleteRecipeHandlerTests : IDisposable
{
    private readonly Guid recipeId = Guid.NewGuid();
    private readonly Guid recipeIngredientId = Guid.NewGuid();

    private readonly DietMenuDbContextFactory factory;
    private readonly NullLogger<DeleteRecipeHandler> logger;
    private readonly Mock<IMediator> mediator;
    private readonly RecipeRepository recipeRepository;

    public DeleteRecipeHandlerTests()
    {
        var family = new FamilyEntity(Guid.NewGuid());

        var ingredientType = new IngredientTypeEntity(Guid.NewGuid())
        {
            FamilyId = family.Id,
        };

        var ingredient = new IngredientEntity(Guid.NewGuid())
        {
            FamilyId = family.Id,
            TypeId = ingredientType.Id,
        };

        var mealType = new MealTypeEntity(Guid.NewGuid())
        {
            FamilyId = family.Id,
        };

        var recipe = new RecipeEntity(this.recipeId)
        {
            FamilyId = family.Id,
            Ingredients = new List<RecipeIngredientEntity>
            {
                new (this.recipeIngredientId)
                {
                    IngredientId = ingredient.Id,
                    Quantity = 1,
                    RecipeId = this.recipeId,
                },
            },
            MealTypeId = mealType.Id,
        };

        this.factory = new DietMenuDbContextFactory(family, ingredientType, ingredient, mealType, recipe);

        this.logger = new NullLogger<DeleteRecipeHandler>();
        this.mediator = new Mock<IMediator>();
        this.recipeRepository = new RecipeRepository(this.factory.Context);
    }

    [TestMethod]
    public async Task ShouldThrowRecipeNotFoundWhenInvalidId()
    {
        //GIVEN
        var command = new DeleteRecipe
        {
            Id = Guid.NewGuid(),
        };

        var commandHandler = new DeleteRecipeHandler(this.logger, this.mediator.Object, this.recipeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<RecipeNotFoundException>()
                .Where(i =>
                    i.Code == ErrorCodes.RECIPE_NOT_FOUND
                    && i.Id == command.Id)
            ;
    }

    [TestMethod]
    public async Task ShouldDeleteRecipe()
    {
        //GIVEN
        var command = new DeleteRecipe
        {
            Id = this.recipeId,
        };

        var commandHandler = new DeleteRecipeHandler(this.logger, this.mediator.Object, this.recipeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<DeletedRecipe>(), It.IsAny<CancellationToken>()), Times.Once);

        var recipe = this.factory.Context.Recipes.SingleOrDefault(i => i.Id == command.Id);
        recipe.Should()
            .BeNull()
            ;

        var ingredients = this.factory.Context.RecipeIngredients.Where(i => i.RecipeId == command.Id);
        ingredients.Should()
            .BeEmpty()
            ;
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }
}
