namespace SilentMike.DietMenu.Core.UnitTests.MealTypes;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.MealTypes.CommandHandlers;
using SilentMike.DietMenu.Core.Application.MealTypes.Commands;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class ImportMealTypesHandlerTests
{
    private readonly Guid familyId = Guid.NewGuid();

    private readonly FamilyRepository familyRepository = new();
    private readonly NullLogger<ImportMealTypesHandler> logger = new();
    private readonly MealTypeRepository mealTypeRepository = new();

    public ImportMealTypesHandlerTests()
    {
        var family = new FamilyEntity(this.familyId);

        this.familyRepository.Save(family);
    }

    [TestMethod]
    public async Task ShouldThrowFamilyNotFoundWhenInvalidId()
    {
        //GIVEN
        var command = new ImportMealTypes
        {
            FamilyId = Guid.NewGuid(),
        };

        var commandHandler = new ImportMealTypesHandler(this.familyRepository, this.logger, this.mealTypeRepository);

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
    public async Task ShouldImportMealTypes()
    {
        //GIVEN
        var command = new ImportMealTypes
        {
            FamilyId = this.familyId,
        };

        var commandHandler = new ImportMealTypesHandler(this.familyRepository, this.logger, this.mealTypeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        var mealTypes = await this.mealTypeRepository.Get(command.FamilyId, CancellationToken.None);
        mealTypes.Should()
            .HaveCount(6)
            .And
            .ContainSingle(i =>
                i.InternalName == "FirstBreakfast"
                && i.Name == "I śniadanie"
                && i.Order == 1)
            .And
            .ContainSingle(i =>
                i.InternalName == "SecondBreakfast"
                && i.Name == "II śniadanie"
                && i.Order == 2)
            .And
            .ContainSingle(i =>
                i.InternalName == "Snack"
                && i.Name == "Przekąska"
                && i.Order == 3)
            .And
            .ContainSingle(i =>
                i.InternalName == "Dinner"
                && i.Name == "Obiad"
                && i.Order == 4)
            .And
            .ContainSingle(i =>
                i.InternalName == "Dessert"
                && i.Name == "Deser"
                && i.Order == 5)
            .And
            .ContainSingle(i =>
                i.InternalName == "Supper"
                && i.Name == "Kolacja"
                && i.Order == 6)
            ;
    }
}
