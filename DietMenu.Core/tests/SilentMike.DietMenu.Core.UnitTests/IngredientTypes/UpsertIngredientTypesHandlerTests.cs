namespace SilentMike.DietMenu.Core.UnitTests.IngredientTypes;

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
using SilentMike.DietMenu.Core.Application.IngredientTypes.CommandHandlers;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Events;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels.ValueModels;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels.ValueModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class UpsertIngredientTypesHandlerTests
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly Guid mealTypeId = Guid.NewGuid();
    private readonly string mealTypeName = "meat";
    private readonly int mealTypeOrder = 2;

    private readonly FamilyRepository familyRepository;
    private readonly NullLogger<UpsertIngredientTypesHandler> logger;
    private readonly Mock<IMediator> mediator;
    private readonly IngredientTypeRepository typeRepository;

    public UpsertIngredientTypesHandlerTests()
    {
        this.familyRepository = new FamilyRepository();
        this.logger = new NullLogger<UpsertIngredientTypesHandler>();
        this.mediator = new Mock<IMediator>();
        this.typeRepository = new IngredientTypeRepository();

        var type = new IngredientTypeEntity(this.mealTypeId)
        {
            FamilyId = familyId,
            Name = this.mealTypeName,
        };

        this.typeRepository.Save(type);

        var family = new FamilyEntity(this.familyId);

        this.familyRepository.Save(family);
    }

    [TestMethod]
    public async Task ShouldThrowFamilyNotFoundWhenInvalidId()
    {
        //GIVEN
        var command = new UpsertIngredientTypes
        {
            FamilyId = Guid.NewGuid(),
        };

        var commandHandler = new UpsertIngredientTypesHandler(this.familyRepository, this.logger, this.mediator.Object, this.typeRepository);

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
    public async Task ShouldThrowValidationExceptionIfNameIsSpacesOnCreate()
    {
        //GIVEN
        var command = new UpsertIngredientTypes
        {
            FamilyId = this.familyId,
            IngredientTypes = new List<IngredientTypeToUpsert>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "  ",
                },
            },
        };

        var commandHandler = new UpsertIngredientTypesHandler(this.familyRepository, this.logger, this.mediator.Object, this.typeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i =>
                    i.Errors.Count == 1
                    && i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_INGREDIENT_TYPES_EMPTY_NAME)
                    && i.Errors[ValidationErrorCodes.UPSERT_INGREDIENT_TYPES_EMPTY_NAME]
                        .Contains(ValidationErrorCodes.UPSERT_INGREDIENT_TYPES_EMPTY_NAME_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldThrowValidationExceptionIfNameIsNullOnCreate()
    {
        //GIVEN
        var command = new UpsertIngredientTypes
        {
            FamilyId = this.familyId,
            IngredientTypes = new List<IngredientTypeToUpsert>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = null,
                },
            },
        };

        var commandHandler = new UpsertIngredientTypesHandler(this.familyRepository, this.logger, this.mediator.Object, this.typeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i =>
                    i.Errors.Count == 1
                    && i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_INGREDIENT_TYPES_EMPTY_NAME)
                    && i.Errors[ValidationErrorCodes.UPSERT_INGREDIENT_TYPES_EMPTY_NAME]
                        .Contains(ValidationErrorCodes.UPSERT_INGREDIENT_TYPES_EMPTY_NAME_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldCreateMealType()
    {
        //GIVEN
        var mealTypeToUpsert = new IngredientTypeToUpsert
        {
            Id = Guid.NewGuid(),
            Name = "milk",
        };

        var command = new UpsertIngredientTypes
        {
            FamilyId = this.familyId,
            IngredientTypes = new List<IngredientTypeToUpsert>
            {
                mealTypeToUpsert,
            },
        };

        var commandHandler = new UpsertIngredientTypesHandler(this.familyRepository, this.logger, this.mediator.Object, this.typeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<UpsertedIngredientTypes>(), It.IsAny<CancellationToken>()), Times.Once);

        var type = await this.typeRepository.Get(command.FamilyId, mealTypeToUpsert.Id);
        type.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(mealTypeToUpsert, opt => opt
                .ComparingByMembers<MealTypeToUpsert>()
                .Excluding(i => i.Id))
            ;

        type!.FamilyId.Should()
            .Be(command.FamilyId)
            ;
        type.InternalName.Should()
            .NotBeEmpty()
            ;
        type.Id.Should()
            .Be(mealTypeToUpsert.Id)
            ;
    }

    [TestMethod]
    public async Task ShouldUpdateMealType()
    {
        //GIVEN
        var mealTypeToUpsert = new IngredientTypeToUpsert
        {
            Id = this.mealTypeId,
            Name = "milk",
        };

        var command = new UpsertIngredientTypes
        {
            FamilyId = this.familyId,
            IngredientTypes = new List<IngredientTypeToUpsert>
            {
                mealTypeToUpsert,
            },
        };

        var commandHandler = new UpsertIngredientTypesHandler(this.familyRepository, this.logger, this.mediator.Object, this.typeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<UpsertedIngredientTypes>(), It.IsAny<CancellationToken>()), Times.Once);

        var type = await this.typeRepository.Get(command.FamilyId, mealTypeToUpsert.Id);
        type.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(mealTypeToUpsert, opt => opt
                .ComparingByMembers<MealTypeToUpsert>()
                .Excluding(i => i.Id))
            ;

        type!.FamilyId.Should()
            .Be(command.FamilyId)
            ;
    }
}
