namespace SilentMike.DietMenu.Core.UnitTests.Ingredients;

using System;
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
using SilentMike.DietMenu.Core.Application.Ingredients.ValueModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class UpsertIngredientHandlerTests : IDisposable
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly Guid ingredientId = Guid.NewGuid();
    private readonly Guid ingredientTypeId = Guid.NewGuid();

    private readonly DietMenuDbContextFactory factory;
    private readonly FamilyRepository familyRepository;
    private readonly IngredientRepository ingredientRepository;
    private readonly NullLogger<UpsertIngredientHandler> logger;
    private readonly Mock<IMediator> mediator;
    private readonly IngredientTypeRepository typeRepository;

    public UpsertIngredientHandlerTests()
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
        this.logger = new NullLogger<UpsertIngredientHandler>();
        this.mediator = new Mock<IMediator>();
        this.typeRepository = new IngredientTypeRepository(this.factory.Context);
    }

    [TestMethod]
    public async Task ShouldThrowFamilyNotFoundWhenInvalidFamilyIdOnUpsertIngredient()
    {
        //
        var command = new UpsertIngredient
        {
            FamilyId = Guid.NewGuid(),
        };

        var commandHandler = new UpsertIngredientHandler(
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
    public async Task ShouldThrowValidationExceptionWhenMissingTypeIdOnCreateOnUpsertIngredient()
    {
        //GIVEN
        var ingredientToUpsert = new IngredientToUpsert
        {
            TypeId = null,
        };

        var command = new UpsertIngredient
        {
            FamilyId = this.familyId,
            Ingredient = ingredientToUpsert,
        };

        var commandHandler = new UpsertIngredientHandler(
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
                .Where(i => i.Code == ErrorCodes.VALIDATION_FAILED
                            && i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_TYPE)
                            && i.Errors[ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_TYPE].Contains(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_TYPE_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldThrowIngredientTypeNotFoundWhenInvalidTypeIdOnUpsertIngredient()
    {
        //GIVEN
        var ingredientToUpsert = new IngredientToUpsert
        {
            Exchanger = 1,
            Name = "name",
            UnitSymbol = "kg",
            TypeId = Guid.NewGuid(),
        };

        var command = new UpsertIngredient
        {
            FamilyId = this.familyId,
            Ingredient = ingredientToUpsert,
        };

        var commandHandler = new UpsertIngredientHandler(
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
    public async Task ShouldThrowValidationExceptionWhenAllParametersAreNullOnCreateOnUpsertIngredient()
    {
        //GIVEN
        var ingredientToUpsert = new IngredientToUpsert
        {
            Exchanger = null,
            Name = null,
            TypeId = this.ingredientTypeId,
            UnitSymbol = null,
        };

        var command = new UpsertIngredient
        {
            FamilyId = this.familyId,
            Ingredient = ingredientToUpsert,
        };

        var commandHandler = new UpsertIngredientHandler(
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
                .Where(i => i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME)
                            && i.Errors[ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME]
                                .Contains(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME_MESSAGE))
                .Where(i => i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT)
                            && i.Errors[ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT]
                                .Contains(ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT_MESSAGE))
                .Where(i => i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_INGREDIENT_INVALID_EXCHANGER)
                            && i.Errors[ValidationErrorCodes.UPSERT_INGREDIENT_INVALID_EXCHANGER]
                                .Contains(ValidationErrorCodes.UPSERT_INGREDIENT_INVALID_EXCHANGER_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldCreateIngredientOnUpsertIngredient()
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

        var command = new UpsertIngredient
        {
            FamilyId = this.familyId,
            Ingredient = ingredientToUpsert,
        };

        var commandHandler = new UpsertIngredientHandler(
            this.familyRepository,
            this.ingredientRepository,
            this.logger,
            this.mediator.Object,
            this.typeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<UpsertedIngredient>(), It.IsAny<CancellationToken>()), Times.Once);

        var ingredient = await this.ingredientRepository.GetAsync(command.FamilyId, ingredientToUpsert.Id);
        ingredient.Should()
            .NotBeNull()
            ;
        ingredient!.Exchanger.Should()
            .Be(ingredientToUpsert.Exchanger)
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
    public async Task ShouldUpdateIngredientOnUpsertIngredient()
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

        var command = new UpsertIngredient
        {
            FamilyId = this.familyId,
            Ingredient = ingredientToUpsert,
        };

        var commandHandler = new UpsertIngredientHandler(
            this.familyRepository,
            this.ingredientRepository,
            this.logger,
            this.mediator.Object,
            this.typeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<UpsertedIngredient>(), It.IsAny<CancellationToken>()), Times.Once);

        var ingredient = await this.ingredientRepository.GetAsync(command.FamilyId, ingredientToUpsert.Id);
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
