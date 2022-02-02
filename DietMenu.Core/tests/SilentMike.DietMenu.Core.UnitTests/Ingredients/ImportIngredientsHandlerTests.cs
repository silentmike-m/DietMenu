namespace SilentMike.DietMenu.Core.UnitTests.Ingredients;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Ingredients.CommandHandlers;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class ImportIngredientsHandlerTests
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly Guid ingredientTypeId = Guid.NewGuid();
    private readonly string ingredientTypeInternalName = "test";

    private readonly FamilyRepository familyRepository = new();
    private readonly NullLogger<ImportIngredientsHandler> logger = new();
    private readonly Mock<IMediator> mediator = new();
    private readonly IngredientRepository ingredientRepository = new();
    private readonly IngredientTypeRepository ingredientTypeRepository = new();

    public ImportIngredientsHandlerTests()
    {
        var family = new FamilyEntity(this.familyId);

        this.familyRepository.Save(family);

        var ingredientType = new IngredientTypeEntity(this.ingredientTypeId)
        {
            FamilyId = this.familyId,
            InternalName = this.ingredientTypeInternalName,
        };

        this.ingredientTypeRepository.Save(ingredientType);
    }

    [TestMethod]
    public async Task ShouldThrowFamilyNotFoundWhenInvalidId()
    {
        //GIVEN
        var command = new ImportIngredients
        {
            FamilyId = Guid.NewGuid(),
        };

        var commandHandler = new ImportIngredientsHandler(this.familyRepository, this.logger, this.mediator.Object, this.ingredientRepository, this.ingredientTypeRepository);

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
    public async Task ShouldImportIngredients()
    {
        //GIVEN
        var ingredientOne = new IngredientEntity(Guid.NewGuid())
        {
            Exchanger = 11.5m,
            FamilyId = this.familyId,
            InternalName = Guid.NewGuid().ToString(),
            IsSystem = true,
            Name = "ingredientOne",
            TypeId = ingredientTypeId,
            UnitSymbol = "szt.",
        };

        var ingredientTwo = new IngredientEntity(Guid.NewGuid())
        {
            Exchanger = 12.1m,
            FamilyId = this.familyId,
            InternalName = Guid.NewGuid().ToString(),
            IsSystem = true,
            Name = "ingredientTwo",
            TypeId = ingredientTypeId,
            UnitSymbol = "kg",
        };

        IReadOnlyList<IngredientEntity> empty = new List<IngredientEntity>();

        IReadOnlyList<IngredientEntity> parsedIngredients = new List<IngredientEntity>
        {
            ingredientOne,
            ingredientTwo,
        };

        this.mediator.Setup(i => i.Send(It.IsAny<ParseIngredientsFromExcelFile>(), It.IsAny<CancellationToken>()))
            .Returns<ParseIngredientsFromExcelFile, CancellationToken>((query, _) =>
            {
                if (query.TypeInternalName == this.ingredientTypeInternalName)
                {
                    return Task.FromResult(parsedIngredients);
                }

                return Task.FromResult(empty);
            });

        var command = new ImportIngredients
        {
            FamilyId = this.familyId,
        };

        var commandHandler = new ImportIngredientsHandler(this.familyRepository, this.logger, this.mediator.Object, this.ingredientRepository, this.ingredientTypeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        var ingredients = await this.ingredientRepository.GetByFamilyId(this.familyId);
        ingredients.Should()
            .HaveCount(2)
            .And
            .Contain(ingredientOne)
            .And
            .Contain(ingredientTwo)
            ;
    }
}
