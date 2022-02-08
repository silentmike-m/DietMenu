namespace SilentMike.DietMenu.Core.UnitTests.MealTypes;

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
using SilentMike.DietMenu.Core.Application.MealTypes.CommandHandlers;
using SilentMike.DietMenu.Core.Application.MealTypes.Commands;
using SilentMike.DietMenu.Core.Application.MealTypes.Events;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels.ValueModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services;
using FamilyRepository = SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services.FamilyRepository;

[TestClass]
public sealed class UpsertMealTypeHandlerTests : IDisposable
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly Guid mealTypeId = Guid.NewGuid();
    private readonly string mealTypeName = "meat";
    private readonly int mealTypeOrder = 2;

    private readonly DietMenuDbContextFactory factory;
    private readonly FamilyRepository familyRepository;
    private readonly NullLogger<UpsertMealTypeHandler> logger;
    private readonly Mock<IMediator> mediator;
    private readonly MealTypeRepository mealTypeRepository;

    public UpsertMealTypeHandlerTests()
    {
        var family = new FamilyEntity(this.familyId);

        var mealType = new MealTypeEntity(this.mealTypeId)
        {
            FamilyId = familyId,
            Name = this.mealTypeName,
            Order = this.mealTypeOrder,
        };

        this.factory = new DietMenuDbContextFactory(family, mealType);
        this.familyRepository = new FamilyRepository(this.factory.Context);
        this.logger = new NullLogger<UpsertMealTypeHandler>();
        this.mealTypeRepository = new MealTypeRepository(this.factory.Context);
        this.mediator = new Mock<IMediator>();
    }

    [TestMethod]
    public async Task ShouldThrowFamilyNotFoundWhenInvalidId()
    {
        //GIVEN
        var command = new UpsertMealType
        {
            FamilyId = Guid.NewGuid(),
        };

        var commandHandler = new UpsertMealTypeHandler(this.familyRepository, this.logger, this.mediator.Object, this.mealTypeRepository);

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
        var command = new UpsertMealType
        {
            FamilyId = this.familyId,
            MealType = new MealTypeToUpsert
            {
                Id = Guid.NewGuid(),
                Name = "  ",
                Order = 2,
            },
        };

        var commandHandler = new UpsertMealTypeHandler(this.familyRepository, this.logger, this.mediator.Object, this.mealTypeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i =>
                    i.Errors.Count == 1
                    && i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_MEAL_TYPE_EMPTY_NAME)
                    && i.Errors[ValidationErrorCodes.UPSERT_MEAL_TYPE_EMPTY_NAME]
                        .Contains(ValidationErrorCodes.UPSERT_MEAL_TYPE_EMPTY_NAME_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldThrowValidationExceptionIfNameIsNullOnCreate()
    {
        //GIVEN
        var command = new UpsertMealType
        {
            FamilyId = this.familyId,
            MealType = new MealTypeToUpsert
            {
                Id = Guid.NewGuid(),
                Name = "  ",
                Order = 2,
            },
        };

        var commandHandler = new UpsertMealTypeHandler(this.familyRepository, this.logger, this.mediator.Object, this.mealTypeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i =>
                    i.Errors.Count == 1
                    && i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_MEAL_TYPE_EMPTY_NAME)
                    && i.Errors[ValidationErrorCodes.UPSERT_MEAL_TYPE_EMPTY_NAME]
                        .Contains(ValidationErrorCodes.UPSERT_MEAL_TYPE_EMPTY_NAME_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldThrowValidationExceptionWhenOrderIsNullOnCreate()
    {
        //GIVEN
        var command = new UpsertMealType
        {
            FamilyId = this.familyId,
            MealType = new MealTypeToUpsert
            {
                Id = Guid.NewGuid(),
                Name = "milk",
                Order = null,
            },
        };

        var commandHandler = new UpsertMealTypeHandler(this.familyRepository, this.logger, this.mediator.Object, this.mealTypeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i =>
                    i.Errors.Count == 1
                    && i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_MEAL_TYPE_INVALID_ORDER)
                    && i.Errors[ValidationErrorCodes.UPSERT_MEAL_TYPE_INVALID_ORDER]
                        .Contains(ValidationErrorCodes.UPSERT_MEAL_TYPE_INVALID_ORDER_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldThrowValidationExceptionWhenOrderIsLessThan1OnCreate()
    {
        //GIVEN
        var command = new UpsertMealType
        {
            FamilyId = this.familyId,
            MealType = new MealTypeToUpsert
            {
                Id = Guid.NewGuid(),
                Name = "milk",
                Order = 0,
            },
        };

        var commandHandler = new UpsertMealTypeHandler(this.familyRepository, this.logger, this.mediator.Object, this.mealTypeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i =>
                    i.Errors.Count == 1
                    && i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_MEAL_TYPE_INVALID_ORDER)
                    && i.Errors[ValidationErrorCodes.UPSERT_MEAL_TYPE_INVALID_ORDER]
                        .Contains(ValidationErrorCodes.UPSERT_MEAL_TYPE_INVALID_ORDER_MESSAGE))
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

        var command = new UpsertMealType
        {
            FamilyId = this.familyId,
            MealType = mealTypeToUpsert,
        };

        var commandHandler = new UpsertMealTypeHandler(this.familyRepository, this.logger, this.mediator.Object, this.mealTypeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<UpsertedMealType>(), It.IsAny<CancellationToken>()), Times.Once);

        var mealType = await this.mealTypeRepository.Get(mealTypeToUpsert.Id);
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
            .Be(mealTypeToUpsert.Id.ToString())
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

        var command = new UpsertMealType
        {
            FamilyId = this.familyId,
            MealType = mealTypeToUpsert,
        };

        var commandHandler = new UpsertMealTypeHandler(this.familyRepository, this.logger, this.mediator.Object, this.mealTypeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<UpsertedMealType>(), It.IsAny<CancellationToken>()), Times.Once);

        var mealType = await this.mealTypeRepository.Get(mealTypeToUpsert.Id);
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

    public void Dispose()
    {
        this.factory.Dispose();
    }
}
