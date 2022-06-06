namespace SilentMike.DietMenu.Core.UnitTests.Families.Processors;

using SilentMike.DietMenu.Core.Application.Families.Models;
using SilentMike.DietMenu.Core.Application.Families.Processors;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Entities;

[TestClass]
public sealed class GetFamilyMealTypesToImportPostProcessorTests
{
    private readonly string dataName = DataNames.MealTypes;

    private readonly NullLogger<GetFamilyMealTypesToImportPostProcessor<GetFamilyDataToImport, FamilyDataToImport>> logger = new();

    [TestMethod]
    public async Task ShouldImportMealTypesOnGetFamilyMealTypesToImportPostProcessor()
    {
        //GIVEN
        var core = new CoreEntity(Guid.NewGuid())
        {
            Versions = new Dictionary<string, string>
            {
                { this.dataName, "Version" },
            },
        };

        var coreMealType = new CoreMealTypeEntity(Guid.NewGuid())
        {
            InternalName = "InternalName",
            Name = "Name",
            Order = 2,
        };

        var family = new FamilyEntity(Guid.NewGuid());

        var request = new GetFamilyDataToImport
        {
            Core = core,
            CoreMealTypes = new List<CoreMealTypeEntity>
            {
                coreMealType,
            },
            Family = family,
        };

        var response = new FamilyDataToImport();

        var processor = new GetFamilyMealTypesToImportPostProcessor<GetFamilyDataToImport, FamilyDataToImport>(logger);

        //WHEN
        await processor.Process(request, response, CancellationToken.None);

        //THEN
        request.Family[this.dataName].Should()
            .Be(core[this.dataName])
            ;

        response.Exceptions.Should()
            .BeEmpty()
            ;
        response.MealTypes.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.FamilyId == request.Family.Id
                && i.Id != coreMealType.Id
                && i.InternalName == coreMealType.InternalName
                && i.IsActive == true
                && i.Name == coreMealType.Name
                && i.Order == coreMealType.Order
            );
    }

    [TestMethod]
    public async Task ShouldUpdateMealTypesOnGetFamilyMealTypesToImportPostProcessor()
    {
        //GIVEN
        var core = new CoreEntity(Guid.NewGuid())
        {
            Versions = new Dictionary<string, string>
            {
                { this.dataName, "Version" },
            },
        };

        var coreMealType = new CoreMealTypeEntity(Guid.NewGuid())
        {
            InternalName = "InternalName",
            Name = "Name",
            Order = 5,
        };

        var family = new FamilyEntity(Guid.NewGuid())
        {
            Versions = new Dictionary<string, string>
            {
                { this.dataName, "Family Version" },
            },
        };

        var familyMealType = new MealTypeEntity(Guid.NewGuid())
        {
            FamilyId = family.Id,
            InternalName = coreMealType.InternalName,
            IsActive = false,
            Name = "family meal type name",
        };

        var request = new GetFamilyDataToImport
        {
            Core = core,
            CoreMealTypes = new List<CoreMealTypeEntity>
            {
                coreMealType,
            },
            Family = family,
        };

        var response = new FamilyDataToImport
        {
            MealTypes = new List<MealTypeEntity>
            {
                familyMealType,
            },
        };

        var processor = new GetFamilyMealTypesToImportPostProcessor<GetFamilyDataToImport, FamilyDataToImport>(logger);

        //WHEN
        await processor.Process(request, response, CancellationToken.None);

        //THEN
        request.Family[this.dataName].Should()
            .Be(core[this.dataName])
            ;

        response.Exceptions.Should()
            .BeEmpty()
            ;
        response.MealTypes.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.FamilyId == request.Family.Id
                && i.Id == familyMealType.Id
                && i.InternalName == coreMealType.InternalName
                && i.IsActive == true
                && i.Name == coreMealType.Name
                && i.Order == coreMealType.Order
            );
    }
}
