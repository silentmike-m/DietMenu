namespace SilentMike.DietMenu.Core.UnitTests.Families.Processors;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Families.Models;
using SilentMike.DietMenu.Core.Application.Families.Processors;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Entities;

[TestClass]
public sealed class GetFamilyIngredientTypesToImportPostProcessorTests
{
    private readonly string dataName = DataNames.IngredientTypes;

    private readonly NullLogger<GetFamilyIngredientTypesToImportPostProcessor<GetFamilyDataToImport, FamilyDataToImport>> logger = new();

    [TestMethod]
    public async Task ShouldImportIngredientTypesOnGetFamilyIngredientTypesToImportPostProcessor()
    {
        //GIVEN
        var core = new CoreEntity(Guid.NewGuid())
        {
            Versions = new Dictionary<string, string>
            {
                { this.dataName, "Version" },
            },
        };

        var coreIngredientType = new CoreIngredientTypeEntity(Guid.NewGuid())
        {
            InternalName = "InternalName",
            Name = "Name",
        };

        var family = new FamilyEntity(Guid.NewGuid());

        var request = new GetFamilyDataToImport
        {
            Core = core,
            CoreIngredientTypes = new List<CoreIngredientTypeEntity>
            {
                coreIngredientType,
            },
            Family = family,
        };

        var response = new FamilyDataToImport();

        var processor = new GetFamilyIngredientTypesToImportPostProcessor<GetFamilyDataToImport, FamilyDataToImport>(logger);

        //WHEN
        await processor.Process(request, response, CancellationToken.None);

        //THEN
        request.Family[this.dataName].Should()
            .Be(core[this.dataName])
            ;

        response.Exceptions.Should()
            .BeEmpty()
            ;
        response.IngredientTypes.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.FamilyId == request.Family.Id
                && i.Id != coreIngredientType.Id
                && i.InternalName == coreIngredientType.InternalName
                && i.IsActive == true
                && i.Name == coreIngredientType.Name
            );
    }

    [TestMethod]
    public async Task ShouldUpdateIngredientTypesOnGetFamilyIngredientTypesToImportPostProcessor()
    {
        //GIVEN
        var core = new CoreEntity(Guid.NewGuid())
        {
            Versions = new Dictionary<string, string>
            {
                { this.dataName, "Version" },
            },
        };

        var coreIngredientType = new CoreIngredientTypeEntity(Guid.NewGuid())
        {
            InternalName = "InternalName",
            Name = "Name",
        };

        var family = new FamilyEntity(Guid.NewGuid())
        {
            Versions = new Dictionary<string, string>
            {
                { this.dataName, "Family Version" },
            },
        };

        var familyIngredientType = new IngredientTypeEntity(Guid.NewGuid())
        {
            FamilyId = family.Id,
            InternalName = coreIngredientType.InternalName,
            IsActive = false,
            Name = "family ingredient type name",
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
        };

        var processor = new GetFamilyIngredientTypesToImportPostProcessor<GetFamilyDataToImport, FamilyDataToImport>(logger);

        //WHEN
        await processor.Process(request, response, CancellationToken.None);

        //THEN
        request.Family[this.dataName].Should()
            .Be(core[this.dataName])
            ;

        response.Exceptions.Should()
            .BeEmpty()
            ;
        response.IngredientTypes.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.FamilyId == request.Family.Id
                && i.Id == familyIngredientType.Id
                && i.InternalName == coreIngredientType.InternalName
                && i.IsActive == true
                && i.Name == coreIngredientType.Name
            );
    }
}
