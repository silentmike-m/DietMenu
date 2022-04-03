namespace SilentMike.DietMenu.Core.UnitTests.EntityFramework;

using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.AutoMapper;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class IngredientTypeReadServiceTests : IDisposable
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly IngredientTypeEntity firstType;
    private readonly IngredientTypeEntity secondType;

    private readonly DietMenuDbContextFactory factory;
    private readonly IngredientTypeReadService service;

    public IngredientTypeReadServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<IngredientTypeProfile>());
        var mapper = config.CreateMapper();

        var family = new FamilyEntity(this.familyId);

        this.firstType = new(Guid.NewGuid())
        {
            FamilyId = this.familyId,
            InternalName = Guid.NewGuid().ToString(),
            Name = "test_name_1",
        };
        this.secondType = new(Guid.NewGuid())
        {
            FamilyId = this.familyId,
            InternalName = Guid.NewGuid().ToString(),
            Name = "test_name_2",
        };

        this.factory = new DietMenuDbContextFactory(family, this.firstType, this.secondType);

        this.service = new IngredientTypeReadService(this.factory.Context, mapper);
    }

    [TestMethod]
    public async Task ShouldReturnIngredientTypes()
    {
        //WHEN
        var result = await this.service.GetIngredientTypesAsync(this.familyId);

        //THEN
        result.Types.Should()
            .HaveCount(2)
            .And
            .Contain(i =>
                i.Id == this.firstType.Id
                && i.Name == this.firstType.Name)
            .And
            .Contain(i =>
                i.Id == this.secondType.Id
                && i.Name == this.secondType.Name)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnPagedIngredientTypesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsPaged = true,
            PageNumber = 0,
            PageSize = 1,
        };

        //WHEN
        var result = await this.service.GetIngredientTypesGridAsync(this.familyId, request);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Id == this.firstType.Id
                && i.Name == this.firstType.Name)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnSortedAndPagedIngredientTypesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsDescending = true,
            IsPaged = true,
            PageNumber = 0,
            PageSize = 1,
            OrderBy = "name",
        };

        //WHEN
        var result = await this.service.GetIngredientTypesGridAsync(this.familyId, request);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Id == this.secondType.Id
                && i.Name == this.secondType.Name)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnFilteredAndSortedAndPagedIngredientTypesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            Filter = this.secondType.Name,
            IsDescending = true,
            IsPaged = true,
            PageNumber = 0,
            PageSize = 1,
            OrderBy = "name",
        };

        //WHEN
        var result = await this.service.GetIngredientTypesGridAsync(this.familyId, request);

        //THEN
        result.Count.Should()
            .Be(1)
            ;
        result.Elements.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Id == this.secondType.Id
                && i.Name == this.secondType.Name)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnNotPagedIngredientTypesGrid()
    {
        //GIVEN
        var request = new GridRequest();

        //WHEN
        var result = await this.service.GetIngredientTypesGridAsync(this.familyId, request);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(2)
            .And
            .Contain(i =>
                i.Id == this.firstType.Id
                && i.Name == this.firstType.Name)
            .And
            .Contain(i =>
                i.Id == this.secondType.Id
                && i.Name == this.secondType.Name)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnSortedNotPagedIngredientTypesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsDescending = true,
            OrderBy = "order",
        };

        //WHEN
        var result = await this.service.GetIngredientTypesGridAsync(this.familyId, request);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(2)
            .And
            .ContainInOrder(new IngredientType
            {
                Id = this.secondType.Id,
                Name = this.secondType.Name,
            }, new IngredientType
            {
                Id = this.firstType.Id,
                Name = this.firstType.Name,
            });
    }

    [TestMethod]
    public async Task ShouldReturnFilteredAndSortedAndNotPagedIngredientTypesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            Filter = "test",
            IsDescending = false,
            OrderBy = "order",
        };

        //WHEN
        var result = await this.service.GetIngredientTypesGridAsync(this.familyId, request);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(2)
            .And
            .ContainInOrder(new IngredientType
            {
                Id = this.firstType.Id,
                Name = this.firstType.Name,
            }, new IngredientType
            {
                Id = this.secondType.Id,
                Name = this.secondType.Name,
            });
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }
}
