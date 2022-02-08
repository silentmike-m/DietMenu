namespace SilentMike.DietMenu.Core.UnitTests.IngredientTypes;

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
using SilentMike.DietMenu.Core.Application.IngredientTypes.CommandHandlers;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Events;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels.ValueModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class UpsertIngredientTypeHandlerTests : IDisposable
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly Guid ingredientTypeId = Guid.NewGuid();
    private readonly string ingredientTypeName = "meat";

    private readonly DietMenuDbContextFactory factory;
    private readonly FamilyRepository familyRepository;
    private readonly NullLogger<UpsertIngredientTypeHandler> logger;
    private readonly Mock<IMediator> mediator;
    private readonly IngredientTypeRepository typeRepository;

    public UpsertIngredientTypeHandlerTests()
    {
        var family = new FamilyEntity(this.familyId);

        var type = new IngredientTypeEntity(this.ingredientTypeId)
        {
            FamilyId = familyId,
            Name = this.ingredientTypeName,
        };

        this.factory = new DietMenuDbContextFactory(type, family);
        this.familyRepository = new FamilyRepository(this.factory.Context);
        this.logger = new NullLogger<UpsertIngredientTypeHandler>();
        this.mediator = new Mock<IMediator>();
        this.typeRepository = new IngredientTypeRepository(this.factory.Context);
    }

    [TestMethod]
    public async Task ShouldThrowFamilyNotFoundWhenInvalidId()
    {
        //GIVEN
        var command = new UpsertIngredientType
        {
            FamilyId = Guid.NewGuid(),
        };

        var commandHandler = new UpsertIngredientTypeHandler(this.familyRepository, this.logger, this.mediator.Object, this.typeRepository);

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
        var command = new UpsertIngredientType
        {
            FamilyId = this.familyId,
            IngredientType = new IngredientTypeToUpsert
            {
                Id = Guid.NewGuid(),
                Name = "  ",
            },
        };

        var commandHandler = new UpsertIngredientTypeHandler(this.familyRepository, this.logger, this.mediator.Object, this.typeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i =>
                    i.Errors.Count == 1
                    && i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_INGREDIENT_TYPE_EMPTY_NAME)
                    && i.Errors[ValidationErrorCodes.UPSERT_INGREDIENT_TYPE_EMPTY_NAME]
                        .Contains(ValidationErrorCodes.UPSERT_INGREDIENT_TYPE_EMPTY_NAME_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldThrowValidationExceptionIfNameIsNullOnCreate()
    {
        //GIVEN
        var command = new UpsertIngredientType
        {
            FamilyId = this.familyId,
            IngredientType = new IngredientTypeToUpsert
            {
                Id = Guid.NewGuid(),
                Name = null,
            },
        };

        var commandHandler = new UpsertIngredientTypeHandler(this.familyRepository, this.logger, this.mediator.Object, this.typeRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i =>
                    i.Errors.Count == 1
                    && i.Errors.ContainsKey(ValidationErrorCodes.UPSERT_INGREDIENT_TYPE_EMPTY_NAME)
                    && i.Errors[ValidationErrorCodes.UPSERT_INGREDIENT_TYPE_EMPTY_NAME]
                        .Contains(ValidationErrorCodes.UPSERT_INGREDIENT_TYPE_EMPTY_NAME_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldCreateIngredientType()
    {
        //GIVEN
        var ingredientTypeToUpsert = new IngredientTypeToUpsert
        {
            Id = Guid.NewGuid(),
            Name = "milk",
        };

        var command = new UpsertIngredientType
        {
            FamilyId = this.familyId,
            IngredientType = ingredientTypeToUpsert,
        };

        var commandHandler = new UpsertIngredientTypeHandler(this.familyRepository, this.logger, this.mediator.Object, this.typeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<UpsertedIngredientType>(), It.IsAny<CancellationToken>()), Times.Once);

        var type = await this.typeRepository.Get(ingredientTypeToUpsert.Id);
        type.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(ingredientTypeToUpsert, opt => opt
                    .ComparingByMembers<IngredientTypeToUpsert>()
                    .Excluding(i => i.Id))
                ;

        type!.FamilyId.Should()
            .Be(command.FamilyId)
            ;
        type.InternalName.Should()
                .Be(ingredientTypeToUpsert.Id.ToString())
                ;
        type.IsSystem.Should()
                .BeFalse()
                ;
        type.Id.Should()
                .Be(ingredientTypeToUpsert.Id)
                ;
    }

    [TestMethod]
    public async Task ShouldUpdateIngredientType()
    {
        //GIVEN
        var ingredientTypeToUpsert = new IngredientTypeToUpsert
        {
            Id = this.ingredientTypeId,
            Name = "milk",
        };

        var command = new UpsertIngredientType
        {
            FamilyId = this.familyId,
            IngredientType = ingredientTypeToUpsert,
        };

        var commandHandler = new UpsertIngredientTypeHandler(this.familyRepository, this.logger, this.mediator.Object, this.typeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<UpsertedIngredientType>(), It.IsAny<CancellationToken>()), Times.Once);

        var type = await this.typeRepository.Get(ingredientTypeToUpsert.Id);
        type.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(ingredientTypeToUpsert, opt => opt
                    .ComparingByMembers<IngredientTypeToUpsert>()
                    .Excluding(i => i.Id))
                ;

        type!.FamilyId.Should()
            .Be(command.FamilyId)
            ;
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }
}
