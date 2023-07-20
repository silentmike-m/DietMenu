namespace SilentMike.DietMenu.Auth.UnitTests.Identity.Services;

using System.Reflection;
using FluentAssertions;
using global::AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Auth.Domain.Entities;
using SilentMike.DietMenu.Auth.Infrastructure;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Services;
using SilentMike.DietMenu.Auth.UnitTests.Helpers;

[TestClass]
public sealed class FamilyRepositoryTests : FakeDietMenuDbContext
{
    private static readonly Family EXISTING_FAMILY = new()
    {
        Id = Guid.NewGuid(),
        Name = "family name",
    };

    private static readonly Family EXISTING_FAMILY_TO_UPDATE = new()
    {
        Id = Guid.NewGuid(),
        Name = "family name",
    };

    private readonly IMapper mapper;

    public FamilyRepositoryTests()
    {
        this.Create(EXISTING_FAMILY, EXISTING_FAMILY_TO_UPDATE);

        var config = new MapperConfiguration(config => config.AddMaps(Assembly.GetAssembly(typeof(DependencyInjection))));
        this.mapper = config.CreateMapper();
    }

    [TestMethod]
    public async Task Should_Create_Family_On_Save_When_Family_Not_Exists()
    {
        //GIVEN
        var entity = new FamilyEntity(Guid.NewGuid())
        {
            Name = "new family name",
        };

        var repository = new FamilyRepository(this.Context!, this.mapper);

        //WHEN
        await repository.SaveAsync(entity, CancellationToken.None);

        //THEN
        var result = await this.Context!.Families.SingleOrDefaultAsync(family => family.Id == entity.Id, CancellationToken.None);

        var expectedResult = new Family
        {
            Id = entity.Id,
            Name = entity.Name,
        };

        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResult, options => options.Excluding(family => family.Key))
            ;
    }

    [TestMethod]
    public async Task Should_Return_Family_On_Get_Family()
    {
        //GIVEN
        var repository = new FamilyRepository(this.Context!, this.mapper);

        //WHEN
        var result = await repository.GetAsync(EXISTING_FAMILY.Id, CancellationToken.None);

        //THEN
        var expectedResult = new FamilyEntity(EXISTING_FAMILY.Id)
        {
            Name = EXISTING_FAMILY.Name,
        };

        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResult)
            ;
    }

    [TestMethod]
    public async Task Should_Return_Null_When_Missing_Family_On_Get_Family()
    {
        //GIVEN
        var repository = new FamilyRepository(this.Context!, this.mapper);

        //WHEN
        var result = await repository.GetAsync(Guid.NewGuid(), CancellationToken.None);

        //THEN
        result.Should()
            .BeNull()
            ;
    }

    [TestMethod]
    public async Task Should_Update_Family_On_Save_When_Family_Exists()
    {
        //GIVEN
        var entity = new FamilyEntity(EXISTING_FAMILY_TO_UPDATE.Id)
        {
            Name = "new family name",
        };

        var repository = new FamilyRepository(this.Context!, this.mapper);

        //WHEN
        await repository.SaveAsync(entity, CancellationToken.None);

        //THEN
        var result = await this.Context!.Families.SingleOrDefaultAsync(family => family.Id == entity.Id, CancellationToken.None);

        var expectedResult = new Family
        {
            Key = EXISTING_FAMILY_TO_UPDATE.Key,
            Id = entity.Id,
            Name = entity.Name,
        };

        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResult)
            ;
    }
}
