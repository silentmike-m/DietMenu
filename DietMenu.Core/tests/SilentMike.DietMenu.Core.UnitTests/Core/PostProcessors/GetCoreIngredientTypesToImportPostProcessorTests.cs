namespace SilentMike.DietMenu.Core.UnitTests.Core.PostProcessors;

using SilentMike.DietMenu.Core.Application.Core.Interfaces;
using SilentMike.DietMenu.Core.Application.Core.Models;
using SilentMike.DietMenu.Core.Application.Core.PostProcessors;
using SilentMike.DietMenu.Core.Application.Core.Queries;
using SilentMike.DietMenu.Core.Domain.Common.Constants;

[TestClass]
public sealed class GetCoreIngredientTypesToImportPostProcessorTests
{
    private const string DATA_VERSION = "INIT";

    private readonly string dataName = DataNames.IngredientTypes;

    private readonly NullLogger<GetCoreIngredientTypesToImportPostProcessor<GetCoreDataToImport, ICoreDataToImport>> logger = new();

    [TestMethod]
    public async Task ShouldImportIngredientTypes()
    {
        //GIVEN
        var request = new GetCoreDataToImport();

        var response = new CoreDataToImport();

        var processor = new GetCoreIngredientTypesToImportPostProcessor<GetCoreDataToImport, ICoreDataToImport>(this.logger);

        //WHEN
        await processor.Process(request, response, CancellationToken.None);

        //THEN
        response.Core[this.dataName].Should()
            .Be(DATA_VERSION)
            ;

        response.IngredientTypes.Should()
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
