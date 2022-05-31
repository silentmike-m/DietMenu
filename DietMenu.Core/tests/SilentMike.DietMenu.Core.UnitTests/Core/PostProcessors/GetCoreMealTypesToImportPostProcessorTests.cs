namespace SilentMike.DietMenu.Core.UnitTests.Core.PostProcessors;

using SilentMike.DietMenu.Core.Application.Core.Interfaces;
using SilentMike.DietMenu.Core.Application.Core.Models;
using SilentMike.DietMenu.Core.Application.Core.PostProcessors;
using SilentMike.DietMenu.Core.Application.Core.Queries;
using SilentMike.DietMenu.Core.Domain.Common.Constants;

[TestClass]
public sealed class GetCoreMealTypesToImportPostProcessorTests
{
    private const string DATA_VERSION = "INIT";

    private readonly string dataName = DataNames.MealTypes;

    private readonly NullLogger<GetCoreMealTypesToImportPostProcessor<GetCoreDataToImport, ICoreDataToImport>> logger = new();

    [TestMethod]
    public async Task ShouldImportIngredientTypes()
    {
        //GIVEN
        var request = new GetCoreDataToImport();

        var response = new CoreDataToImport();

        var processor = new GetCoreMealTypesToImportPostProcessor<GetCoreDataToImport, ICoreDataToImport>(this.logger);

        //WHEN
        await processor.Process(request, response, CancellationToken.None);

        //THEN
        response.Core[this.dataName].Should()
            .Be(DATA_VERSION)
            ;

        response.MealTypes.Should()
            .HaveCount(6)
            .And
            .ContainSingle(i =>
                i.InternalName == MealTypeNames.FirstBreakfast
                && i.Name == "I śniadanie"
                && i.Order == 1)
            .And
            .ContainSingle(i =>
                i.InternalName == MealTypeNames.SecondBreakfast
                && i.Name == "II śniadanie"
                && i.Order == 2)
            .And
            .ContainSingle(i =>
                i.InternalName == MealTypeNames.Snack
                && i.Name == "Przekąska"
                && i.Order == 3)
            .And
            .ContainSingle(i =>
                i.InternalName == MealTypeNames.Dinner
                && i.Name == "Obiad"
                && i.Order == 4)
            .And
            .ContainSingle(i =>
                i.InternalName == MealTypeNames.Dessert
                && i.Name == "Deser"
                && i.Order == 5)
            .And
            .ContainSingle(i =>
                i.InternalName == MealTypeNames.Supper
                && i.Name == "Kolacja"
                && i.Order == 6)
            ;
    }
}
