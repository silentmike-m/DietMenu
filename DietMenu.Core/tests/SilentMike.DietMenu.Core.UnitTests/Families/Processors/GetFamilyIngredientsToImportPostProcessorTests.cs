namespace SilentMike.DietMenu.Core.UnitTests.Families.Processors;

using SilentMike.DietMenu.Core.Application.Exceptions.IngredientTypes;
using SilentMike.DietMenu.Core.Application.Families.Models;
using SilentMike.DietMenu.Core.Application.Families.Processors;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Domain.Entities;

[TestClass]
public sealed class GetFamilyIngredientsToImportPostProcessorTests
{
    private readonly NullLogger<GetFamilyIngredientsToImportPostProcessor<GetFamilyDataToImport, FamilyDataToImport>> logger = new();

    [TestMethod]
    public async Task ShouldReturnIngredientTypeNotFoundExceptionWhenMissingFamilyIngredientTypeOnGetFamilyIngredientsToImportPostProcessor()
    {
        //GIVEN
        var coreIngredientType = new CoreIngredientTypeEntity(Guid.NewGuid())
        {
            InternalName = "Fruit",
        };

        var coreIngredient = new CoreIngredientEntity(Guid.NewGuid())
        {
            Exchanger = 12.5m,
            InternalName = "InternalName",
            IsActive = true,
            Name = "Name",
            TypeId = coreIngredientType.Id,
            UnitSymbol = "kg",
        };

        var core = new CoreEntity(Guid.NewGuid())
        {
            Versions = new Dictionary<string, string>
            {
                { coreIngredientType.InternalName, "Version" },
            },
        };

        var family = new FamilyEntity(Guid.NewGuid());

        var request = new GetFamilyDataToImport
        {
            Core = core,
            CoreIngredientTypes = new List<CoreIngredientTypeEntity>
            {
                coreIngredientType,
            },
            CoreIngredients = new List<CoreIngredientEntity>
            {
                coreIngredient,
            },
            Family = family,
        };

        var response = new FamilyDataToImport();

        var processor = new GetFamilyIngredientsToImportPostProcessor<GetFamilyDataToImport, FamilyDataToImport>(logger);

        //WHEN
        await processor.Process(request, response, CancellationToken.None);

        //THEN
        request.Family[coreIngredientType.InternalName].Should()
            .NotBe(core[coreIngredientType.InternalName])
            ;

        response.Exceptions.Should()
            .HaveCount(1)
            .And
            .ContainKey(coreIngredientType.InternalName)
            ;

        response.Exceptions[coreIngredientType.InternalName].Should()
            .ContainItemsAssignableTo<IngredientTypeNotFoundException>()
            ;
    }

    [TestMethod]
    public async Task ShouldImportIngredientsOnGetFamilyIngredientsToImportPostProcessor()
    {
        //GIVEN
        var coreIngredientType = new CoreIngredientTypeEntity(Guid.NewGuid())
        {
            InternalName = "Fruit",
        };

        var coreIngredient = new CoreIngredientEntity(Guid.NewGuid())
        {
            Exchanger = 12.5m,
            InternalName = "InternalName",
            IsActive = true,
            Name = "Name",
            TypeId = coreIngredientType.Id,
            UnitSymbol = "kg",
        };

        var core = new CoreEntity(Guid.NewGuid())
        {
            Versions = new Dictionary<string, string>
            {
                { coreIngredientType.InternalName, "Version" },
            },
        };

        var family = new FamilyEntity(Guid.NewGuid());

        var familyIngredientType = new IngredientTypeEntity(Guid.NewGuid())
        {
            InternalName = coreIngredientType.InternalName,
        };

        var request = new GetFamilyDataToImport
        {
            Core = core,
            CoreIngredientTypes = new List<CoreIngredientTypeEntity>
            {
                coreIngredientType,
            },
            CoreIngredients = new List<CoreIngredientEntity>
            {
                coreIngredient,
            },
            Family = family,
        };

        var response = new FamilyDataToImport
        {
            IngredientTypes = new List<IngredientTypeEntity>
            {
                familyIngredientType,
            },
        };

        var processor = new GetFamilyIngredientsToImportPostProcessor<GetFamilyDataToImport, FamilyDataToImport>(logger);

        //WHEN
        await processor.Process(request, response, CancellationToken.None);

        //THEN
        request.Family[coreIngredientType.InternalName].Should()
            .Be(core[coreIngredientType.InternalName])
            ;

        response.Exceptions.Should()
            .BeEmpty()
            ;
        response.Ingredients.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Exchanger == coreIngredient.Exchanger
                && i.FamilyId == request.Family.Id
                && i.Id != coreIngredient.Id
                && i.InternalName == coreIngredient.InternalName
                && i.IsActive == true
                && i.IsSystem == true
                && i.Name == coreIngredient.Name
                && i.TypeId == familyIngredientType.Id
                && i.UnitSymbol == coreIngredient.UnitSymbol
            );
    }

    [TestMethod]
    public async Task ShouldUpdateIngredientOnGetFamilyIngredientsToImportPostProcessor()
    {
        //GIVEN
        var coreIngredientType = new CoreIngredientTypeEntity(Guid.NewGuid())
        {
            InternalName = "Fruit",
        };

        var coreIngredient = new CoreIngredientEntity(Guid.NewGuid())
        {
            Exchanger = 12.5m,
            InternalName = "InternalName",
            IsActive = true,
            Name = "Name",
            TypeId = coreIngredientType.Id,
            UnitSymbol = "kg",
        };

        var core = new CoreEntity(Guid.NewGuid())
        {
            Versions = new Dictionary<string, string>
            {
                { coreIngredientType.InternalName, "Version" },
            },
        };

        var family = new FamilyEntity(Guid.NewGuid())
        {
            Versions = new Dictionary<string, string>
            {
                { coreIngredientType.InternalName, "Family Version" },
            },
        };

        var familyIngredientType = new IngredientTypeEntity(Guid.NewGuid())
        {
            InternalName = coreIngredientType.InternalName,
        };

        var familyIngredient = new IngredientEntity(Guid.NewGuid())
        {
            Exchanger = 1m,
            FamilyId = family.Id,
            InternalName = "InternalName",
            IsActive = false,
            IsSystem = true,
            Name = "family Name",
            TypeId = familyIngredientType.Id,
            UnitSymbol = "szt",
        };

        var request = new GetFamilyDataToImport
        {
            Core = core,
            CoreIngredientTypes = new List<CoreIngredientTypeEntity>
            {
                coreIngredientType,
            },
            CoreIngredients = new List<CoreIngredientEntity>
            {
                coreIngredient,
            },
            Family = family,
        };

        var response = new FamilyDataToImport
        {
            IngredientTypes = new List<IngredientTypeEntity>
            {
                familyIngredientType,
            },
            Ingredients = new List<IngredientEntity>
            {
                familyIngredient,
            },
        };

        var processor = new GetFamilyIngredientsToImportPostProcessor<GetFamilyDataToImport, FamilyDataToImport>(logger);

        //WHEN
        await processor.Process(request, response, CancellationToken.None);

        //THEN
        request.Family[coreIngredientType.InternalName].Should()
            .Be(core[coreIngredientType.InternalName])
            ;

        response.Exceptions.Should()
            .BeEmpty()
            ;
        response.Ingredients.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Exchanger == coreIngredient.Exchanger
                && i.FamilyId == request.Family.Id
                && i.Id != coreIngredient.Id
                && i.InternalName == coreIngredient.InternalName
                && i.IsActive == true
                && i.IsSystem == true
                && i.Name == coreIngredient.Name
                && i.TypeId == familyIngredientType.Id
                && i.UnitSymbol == coreIngredient.UnitSymbol
            );
    }

    [TestMethod]
    public async Task ShouldDeactivateIngredientOnGetFamilyIngredientsToImportPostProcessor()
    {
        //GIVEN
        var coreIngredientType = new CoreIngredientTypeEntity(Guid.NewGuid())
        {
            InternalName = "Fruit",
        };

        var core = new CoreEntity(Guid.NewGuid())
        {
            Versions = new Dictionary<string, string>
            {
                { coreIngredientType.InternalName, "Version" },
            },
        };

        var family = new FamilyEntity(Guid.NewGuid())
        {
            Versions = new Dictionary<string, string>
            {
                { coreIngredientType.InternalName, "Family Version" },
            },
        };

        var familyIngredientType = new IngredientTypeEntity(Guid.NewGuid())
        {
            InternalName = coreIngredientType.InternalName,
        };

        var familyIngredient = new IngredientEntity(Guid.NewGuid())
        {
            Exchanger = 1m,
            FamilyId = family.Id,
            InternalName = "InternalName",
            IsActive = true,
            IsSystem = true,
            Name = "family Name",
            TypeId = familyIngredientType.Id,
            UnitSymbol = "szt",
        };

        var request = new GetFamilyDataToImport
        {
            Core = core,
            CoreIngredientTypes = new List<CoreIngredientTypeEntity>
            {
                coreIngredientType,
            },
            Family = family,
        };

        var response = new FamilyDataToImport
        {
            IngredientTypes = new List<IngredientTypeEntity>
            {
                familyIngredientType,
            },
            Ingredients = new List<IngredientEntity>
            {
                familyIngredient,
            },
        };

        var processor = new GetFamilyIngredientsToImportPostProcessor<GetFamilyDataToImport, FamilyDataToImport>(logger);

        //WHEN
        await processor.Process(request, response, CancellationToken.None);

        //THEN
        request.Family[coreIngredientType.InternalName].Should()
            .Be(core[coreIngredientType.InternalName])
            ;

        response.Exceptions.Should()
            .BeEmpty()
            ;
        response.Ingredients.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Exchanger == familyIngredient.Exchanger
                && i.FamilyId == request.Family.Id
                && i.Id == familyIngredient.Id
                && i.InternalName == familyIngredient.InternalName
                && i.IsActive == false
                && i.IsSystem == true
                && i.Name == familyIngredient.Name
                && i.TypeId == familyIngredientType.Id
                && i.UnitSymbol == familyIngredient.UnitSymbol
            );
    }
}
