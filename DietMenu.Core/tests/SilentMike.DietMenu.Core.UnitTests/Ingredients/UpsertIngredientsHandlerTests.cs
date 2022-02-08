namespace SilentMike.DietMenu.Core.UnitTests.Ingredients;

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
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Exceptions.IngredientTypes;
using SilentMike.DietMenu.Core.Application.Ingredients.CommandHandlers;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Application.Ingredients.Events;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels.ValueModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class UpsertIngredientsHandlerTests : IDisposable
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly Guid ingredientId = Guid.NewGuid();
    private readonly Guid ingredientTypeId = Guid.NewGuid();

    private readonly DietMenuDbContextFactory factory;
    private readonly FamilyRepository familyRepository;
    private readonly IngredientRepository ingredientRepository;
    private readonly NullLogger<UpsertIngredientsHandler> logger;
    private readonly Mock<IMediator> mediator;
    private readonly IngredientTypeRepository typeRepository;

    public UpsertIngredientsHandlerTests()
    {
        var family = new FamilyEntity(this.familyId);

        var ingredientType = new IngredientTypeEntity(this.ingredientTypeId)
        {
            FamilyId = this.familyId,
        };

        var ingredient = new IngredientEntity(this.ingredientId)
        {
            FamilyId = this.familyId,
            TypeId = this.ingredientTypeId,
        };

        this.factory = new DietMenuDbContextFactory(family, ingredient, ingredientType);
        this.familyRepository = new FamilyRepository(this.factory.Context);
        this.ingredientRepository = new IngredientRepository(this.factory.Context);
        this.logger = new NullLogger<UpsertIngredientsHandler>();
        this.mediator = new Mock<IMediator>();
        this.typeRepository = new IngredientTypeRepository(this.factory.Context);
    }

    [TestMethod]
    public async Task ShouldThrowFamilyNotFoundWhenInvalidFamilyId()
    {
        //
        var command = new UpsertIngredients
        {
            FamilyId = Guid.NewGuid(),
        };

        var commandHandler = new UpsertIngredientsHandler(
            this.familyRepository,
            this.ingredientRepository,
            this.logger,
            this.mediator.Object,
            this.typeRepository);

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
    public async Task ShouldThrowIngredientTypeNotFoundWhenInvalidTypeId()
    {
        //GIVEN
        var ingredientToUpsert = new IngredientToUpsert
        {
            TypeId = Guid.NewGuid(),
        };

        var command = new UpsertIngredients
        {
            FamilyId = this.familyId,
            Ingredients = new List<IngredientToUpsert>
            {
                ingredientToUpsert,
            },
        };

        var commandHandler = new UpsertIngredientsHandler(
            this.familyRepository,
            this.ingredientRepository,
            this.logger,
            this.mediator.Object,
            this.typeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<IngredientTypeNotFoundException>()
                .Where(i =>
                    i.Code == ErrorCodes.INGREDIENT_TYPE_NOT_FOUND
                    && i.Id == ingredientToUpsert.TypeId)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowValidationExceptionWhenAllParametersAreNullOnCreate()
    {
        //GIVEN
        var ingredientToUpsert = new IngredientToUpsert
        {
            Exchanger = null,
            Name = null,
            TypeId = this.ingredientTypeId,
            UnitSymbol = null,
        };

        var command = new UpsertIngredients
        {
            FamilyId = this.familyId,
            Ingredients = new List<IngredientToUpsert>
            {
                ingredientToUpsert,
            },
        };

        var commandHandler = new UpsertIngredientsHandler(
            this.familyRepository,
            this.ingredientRepository,
            this.logger,
            this.mediator.Object,
            this.typeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i => i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_NAME)
                            && i.Errors[ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_NAME]
                                .Contains(ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_NAME_MESSAGE))
                .Where(i => i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_UNIT)
                            && i.Errors[ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_UNIT]
                                .Contains(ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_UNIT_MESSAGE))
                .Where(i => i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_INGREDIENTS_INVALID_EXCHANGER)
                            && i.Errors[ValidationErrorCodes.UPSERT_INGREDIENTS_INVALID_EXCHANGER]
                                .Contains(ValidationErrorCodes.UPSERT_INGREDIENTS_INVALID_EXCHANGER_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldCreateIngredient()
    {
        //GIVEN
        var ingredientToUpsert = new IngredientToUpsert
        {
            Exchanger = 0.5m,
            Id = Guid.NewGuid(),
            Name = "name",
            TypeId = this.ingredientTypeId,
            UnitSymbol = "kg",
        };

        var command = new UpsertIngredients
        {
            FamilyId = this.familyId,
            Ingredients = new List<IngredientToUpsert>
            {
                ingredientToUpsert,
            },
        };

        var commandHandler = new UpsertIngredientsHandler(
            this.familyRepository,
            this.ingredientRepository,
            this.logger,
            this.mediator.Object,
            this.typeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<UpsertedIngredients>(), It.IsAny<CancellationToken>()), Times.Once);

        var ingredient = await this.ingredientRepository.Get(ingredientToUpsert.Id);
        ingredient.Should()
            .NotBeNull()
            ;
        ingredient!.Exchanger.Should()
            .Be(ingredientToUpsert.Exchanger)
            ;
        ingredient.IsSystem.Should()
            .BeFalse()
            ;
        ingredient.Name.Should()
            .Be(ingredientToUpsert.Name)
            ;
        ingredient.InternalName.Should()
            .Be(ingredientToUpsert.Id.ToString()
            );
        ingredient.TypeId.Should()
            .Be(ingredientToUpsert.TypeId.Value)
            ;
        ingredient.UnitSymbol.Should()
            .Be(ingredientToUpsert.UnitSymbol)
            ;
    }

    [TestMethod]
    public async Task ShouldUpdateIngredient()
    {
        //GIVEN
        var ingredientToUpsert = new IngredientToUpsert
        {
            Exchanger = 0.5m,
            Id = this.ingredientId,
            Name = "name",
            TypeId = this.ingredientTypeId,
            UnitSymbol = "kg",
        };

        var command = new UpsertIngredients
        {
            FamilyId = this.familyId,
            Ingredients = new List<IngredientToUpsert>
            {
                ingredientToUpsert,
            },
        };

        var commandHandler = new UpsertIngredientsHandler(
            this.familyRepository,
            this.ingredientRepository,
            this.logger,
            this.mediator.Object,
            this.typeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<UpsertedIngredients>(), It.IsAny<CancellationToken>()), Times.Once);

        var ingredient = await this.ingredientRepository.Get(ingredientToUpsert.Id);
        ingredient.Should()
            .NotBeNull()
            ;
        ingredient!.Exchanger.Should()
            .Be(ingredientToUpsert.Exchanger)
            ;
        ingredient.Name.Should()
            .Be(ingredientToUpsert.Name)
            ;
        ingredient.TypeId.Should()
            .Be(ingredientToUpsert.TypeId.Value)
            ;
        ingredient.UnitSymbol.Should()
            .Be(ingredientToUpsert.UnitSymbol)
            ;
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }
}
