namespace SilentMike.DietMenu.Core.UnitTests.MealTypes;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.MealTypes.CommandHandlers;
using SilentMike.DietMenu.Core.Application.MealTypes.Commands;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class ImportMealTypesHandlerTests
{
    private readonly NullLogger<ImportMealTypesHandler> logger = new();
    private readonly MealTypeRepository repository = new();

    [TestMethod]
    public async Task ShouldImportMealTypes()
    {
        //GIVEN
        var command = new ImportMealTypes
        {
            FamilyId = Guid.NewGuid(),
        };

        var commandHandler = new ImportMealTypesHandler(this.logger, this.repository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        var mealTypes = await this.repository.Get(command.FamilyId, CancellationToken.None);
        mealTypes.Should()
            .HaveCount(6)
            .And
            .ContainSingle(i => i.InternalName == "FirstBreakfast" && i.Name == "I śniadanie")
            .And
            .ContainSingle(i => i.InternalName == "SecondBreakfast" && i.Name == "II śniadanie")
            .And
            .ContainSingle(i => i.InternalName == "Dinner" && i.Name == "Obiad")
            .And
            .ContainSingle(i => i.InternalName == "Supper" && i.Name == "Kolacja")
            .And
            .ContainSingle(i => i.InternalName == "Snack" && i.Name == "Przekąska")
            .And
            .ContainSingle(i => i.InternalName == "Dessert" && i.Name == "Deser")
            ;
    }
}
