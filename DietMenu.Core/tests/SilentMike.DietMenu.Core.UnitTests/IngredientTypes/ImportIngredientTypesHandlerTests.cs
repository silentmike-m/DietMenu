namespace SilentMike.DietMenu.Core.UnitTests.IngredientTypes;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.IngredientTypes.CommandHandlers;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class ImportIngredientTypesHandlerTests
{
    private readonly Guid familyId = Guid.NewGuid();

    private readonly FamilyRepository familyRepository = new();
    private readonly NullLogger<ImportIngredientTypesHandler> logger = new();
    private readonly IngredientTypeRepository typeRepository = new();

    public ImportIngredientTypesHandlerTests()
    {
        var family = new FamilyEntity(this.familyId);

        this.familyRepository.Save(family);
    }

    [TestMethod]
    public async Task ShouldThrowFamilyNotFoundWhenInvalidId()
    {
        //GIVEN
        var command = new ImportIngredientTypes
        {
            FamilyId = Guid.NewGuid(),
        };

        var commandHandler = new ImportIngredientTypesHandler(this.familyRepository, this.logger, this.typeRepository);

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
    public async Task ShouldImportIngredientTypes()
    {
        //GIVEN
        var command = new ImportIngredientTypes
        {
            FamilyId = this.familyId,
        };

        var commandHandler = new ImportIngredientTypesHandler(this.familyRepository, this.logger, this.typeRepository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        var types = await this.typeRepository.GetByFamilyId(command.FamilyId, CancellationToken.None);
        types.Should()
            .HaveCount(7)
            .And
            .ContainSingle(i =>
                i.InternalName == "ComplexCarbohydrate"
                && i.Name == "Węglowodan złożony")
            .And
            .ContainSingle(i =>
                i.InternalName == "Fruit"
                && i.Name == "Owoc")
            .And
            .ContainSingle(i =>
                i.InternalName == "HealthyFat"
                && i.Name == "Zdrowy tłuszcz")
            .And
            .ContainSingle(i =>
                i.InternalName == "HighFatProtein"
                && i.Name == "Białko wysokotłuszczowe")
            .And
            .ContainSingle(i =>
                i.InternalName == "LowFatProtein"
                && i.Name == "Białko niskotłuszczowe")
            .And
            .ContainSingle(i =>
                i.InternalName == "MediumFatProtein"
                && i.Name == "Białko średniotłuszczowe")
            .And
            .ContainSingle(i =>
                i.InternalName == "Other"
                && i.Name == "Inne")
            ;
    }
}
