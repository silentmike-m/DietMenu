namespace SilentMike.DietMenu.Core.UnitTests.MealTypes;

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
using SilentMike.DietMenu.Core.Application.MealTypes.CommandHandlers;
using SilentMike.DietMenu.Core.Application.MealTypes.Commands;
using SilentMike.DietMenu.Core.Application.MealTypes.Events;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels.ValueModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class UpsertMealTypesHandlerTests
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly Guid mealTypeId = Guid.NewGuid();
    private readonly string mealTypeName = "meat";
    private readonly int mealTypeOrder = 2;

    private readonly FamilyRepository familyRepository;
    private readonly NullLogger<UpsertMealTypesHandler> logger;
    private readonly Mock<IMediator> mediator;
    private readonly MealTypeRepository mealTypeRepository;

    public UpsertMealTypesHandlerTests()
    {
        this.familyRepository = new FamilyRepository();
        this.logger = new NullLogger<UpsertMealTypesHandler>();
        this.mediator = new Mock<IMediator>();
        this.mealTypeRepository = new MealTypeRepository();

        var mealType = new MealTypeEntity(this.mealTypeId)
        {
            FamilyId = familyId,
            Name = this.mealTypeName,
            Order = this.mealTypeOrder,
        };

        this.mealTypeRepository.Save(mealType);

        var family = new FamilyEntity(this.familyId);

        this.familyRepository.Save(family);
    }

    [TestMethod]
    public async Task ShouldThrowFamilyNotFoundWhenInvalidId()
    {
        //GIVEN
        var command = new UpsertMealTypes
        {
            FamilyId = Guid.NewGuid(),
        };

        var commandHandler = new UpsertMealTypesHandler(this.familyRepository, this.logger, this.mediator.Object, this.mealTypeRepository);

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
        var command = new UpsertMealTypes
        {
            FamilyId = this.familyId,
            MealTypes = new List<MealTypeToUpsert>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "  ",
                    Order = 2,
                },
            },
        };

        var commandHandler = new UpsertMealTypesHandler(this.familyRepository, this.logger, this.mediator.Object, this.mealTypeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i =>
                    i.Errors.Count == 1
                    && i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_MEAL_TYPES_EMPTY_NAME)
                    && i.Errors[ValidationErrorCodes.UPSERT_MEAL_TYPES_EMPTY_NAME]
                        .Contains(ValidationErrorCodes.UPSERT_MEAL_TYPES_EMPTY_NAME_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldThrowValidationExceptionIfNameIsNullOnCreate()
    {
        //GIVEN
        var command = new UpsertMealTypes
        {
            FamilyId = this.familyId,
            MealTypes = new List<MealTypeToUpsert>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = null,
                    Order = 2,
                },
            },
        };

        var commandHandler = new UpsertMealTypesHandler(this.familyRepository, this.logger, this.mediator.Object, this.mealTypeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i =>
                    i.Errors.Count == 1
                    && i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_MEAL_TYPES_EMPTY_NAME)
                    && i.Errors[ValidationErrorCodes.UPSERT_MEAL_TYPES_EMPTY_NAME]
                        .Contains(ValidationErrorCodes.UPSERT_MEAL_TYPES_EMPTY_NAME_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldThrowValidationExceptionWhenOrderIsNullOnCreate()
    {
        //GIVEN
        var command = new UpsertMealTypes
        {
            FamilyId = this.familyId,
            MealTypes = new List<MealTypeToUpsert>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "milk",
                    Order = null,
                },
            },
        };

        var commandHandler = new UpsertMealTypesHandler(this.familyRepository, this.logger, this.mediator.Object, this.mealTypeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i =>
                    i.Errors.Count == 1
                    && i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_MEAL_TYPES_INVALID_ORDER)
                    && i.Errors[ValidationErrorCodes.UPSERT_MEAL_TYPES_INVALID_ORDER]
                        .Contains(ValidationErrorCodes.UPSERT_MEAL_TYPES_INVALID_ORDER_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldThrowValidationExceptionWhenOrderIsLessThan1OnCreate()
    {
        //GIVEN
        var command = new UpsertMealTypes
        {
            FamilyId = this.familyId,
            MealTypes = new List<MealTypeToUpsert>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "milk",
                    Order = 0,
                },
            },
        };

        var commandHandler = new UpsertMealTypesHandler(this.familyRepository, this.logger, this.mediator.Object, this.mealTypeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i =>
                    i.Errors.Count == 1
                    && i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_MEAL_TYPES_INVALID_ORDER)
                    && i.Errors[ValidationErrorCodes.UPSERT_MEAL_TYPES_INVALID_ORDER]
                        .Contains(ValidationErrorCodes.UPSERT_MEAL_TYPES_INVALID_ORDER_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldCreateMealType()
    {
        //GIVEN
        var mealTypeToUpsert = new MealTypeToUpsert
        {
            Id = Guid.NewGuid(),
            Name = "milk",
            Order = 1,
        };

        var command = new UpsertMealTypes
        {
            FamilyId = this.familyId,
            MealTypes = new List<MealTypeToUpsert>
            {
                mealTypeToUpsert,
            },
        };

        var commandHandler = new UpsertMealTypesHandler(this.familyRepository, this.logger, this.mediator.Object, this.mealTypeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<UpsertedMealTypes>(), It.IsAny<CancellationToken>()), Times.Once);

        var mealType = await this.mealTypeRepository.Get(command.FamilyId, mealTypeToUpsert.Id);
        mealType.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(mealTypeToUpsert, opt => opt
                .ComparingByMembers<MealTypeToUpsert>()
                .Excluding(i => i.Id))
            ;

        mealType!.FamilyId.Should()
            .Be(command.FamilyId)
            ;
        mealType.InternalName.Should()
            .NotBeEmpty()
            ;
        mealType.Id.Should()
            .Be(mealTypeToUpsert.Id)
            ;
    }

    [TestMethod]
    public async Task ShouldUpdateMealType()
    {
        //GIVEN
        var mealTypeToUpsert = new MealTypeToUpsert
        {
            Id = this.mealTypeId,
            Name = "milk",
            Order = 1,
        };

        var command = new UpsertMealTypes
        {
            FamilyId = this.familyId,
            MealTypes = new List<MealTypeToUpsert>
            {
                mealTypeToUpsert,
            },
        };

        var commandHandler = new UpsertMealTypesHandler(this.familyRepository, this.logger, this.mediator.Object, this.mealTypeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<UpsertedMealTypes>(), It.IsAny<CancellationToken>()), Times.Once);

        var mealType = await this.mealTypeRepository.Get(command.FamilyId, mealTypeToUpsert.Id);
        mealType.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(mealTypeToUpsert, opt => opt
                .ComparingByMembers<MealTypeToUpsert>()
                .Excluding(i => i.Id))
            ;

        mealType!.FamilyId.Should()
            .Be(command.FamilyId)
            ;
    }
}
