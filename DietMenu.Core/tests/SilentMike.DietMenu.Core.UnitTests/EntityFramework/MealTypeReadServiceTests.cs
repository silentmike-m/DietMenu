namespace SilentMike.DietMenu.Core.UnitTests.EntityFramework;

using AutoMapper;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.AutoMapper;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using DietMenuDbContextFactory = SilentMike.DietMenu.Core.UnitTests.Services.DietMenuDbContextFactory;

[TestClass]
public sealed class MealTypeReadServiceTests : IDisposable
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly MealTypeRow firstMealType;
    private readonly MealTypeRow secondMealType;

    private readonly DietMenuDbContextFactory factory;
    private readonly MealTypeReadService service;

    public MealTypeReadServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MealTypeRowProfile>());
        var mapper = config.CreateMapper();

        var family = new FamilyEntity(this.familyId);

        this.firstMealType = new()
        {
            FamilyId = this.familyId,
            Id = Guid.NewGuid(),
            IsActive = true,
            Name = "test_name_1",
            Order = 1,
        };
        this.secondMealType = new()
        {
            FamilyId = this.familyId,
            Id = Guid.NewGuid(),
            IsActive = true,
            Name = "test_name_2",
            Order = 2,
        };

        this.factory = new DietMenuDbContextFactory(family, this.firstMealType, this.secondMealType);

        this.service = new MealTypeReadService(this.factory.Context, mapper);
    }

    [TestMethod]
    public async Task ShouldReturnPagedMealTypesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsPaged = true,
            PageNumber = 1,
            PageSize = 1,
        };

        //WHEN
        var result = await this.service.GetMealTypesGridAsync(this.familyId, request);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Id == this.secondMealType.Id
                && i.Name == this.secondMealType.Name
                && i.Order == this.secondMealType.Order)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnSortedAndPagedMealTypesGrid()
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
        var result = await this.service.GetMealTypesGridAsync(this.familyId, request);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Id == this.secondMealType.Id
                && i.Name == this.secondMealType.Name
                && i.Order == this.secondMealType.Order)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnFilteredAndSortedAndPagedMealTypesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            Filter = this.secondMealType.Name,
            IsDescending = true,
            IsPaged = true,
            PageNumber = 0,
            PageSize = 1,
            OrderBy = "name",
        };

        //WHEN
        var result = await this.service.GetMealTypesGridAsync(this.familyId, request);

        //THEN
        result.Count.Should()
            .Be(1)
            ;
        result.Elements.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Id == this.secondMealType.Id
                && i.Name == this.secondMealType.Name
                && i.Order == this.secondMealType.Order)
            ;
    }
    [TestMethod]
    public async Task ShouldReturnNotPagedMealTypesGrid()
    {
        //GIVEN
        var request = new GridRequest();

        //WHEN
        var result = await this.service.GetMealTypesGridAsync(this.familyId, request);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(2)
            .And
            .Contain(i =>
                i.Id == this.firstMealType.Id
                && i.Name == this.firstMealType.Name
                && i.Order == this.firstMealType.Order)
            .And
            .Contain(i =>
                i.Id == this.secondMealType.Id
                && i.Name == this.secondMealType.Name
                && i.Order == this.secondMealType.Order)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnSortedNotPagedResult()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsDescending = true,
            OrderBy = "order",
        };

        //WHEN
        var result = await this.service.GetMealTypesGridAsync(this.familyId, request);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(2)
            .And
            .ContainInOrder(new MealType
            {
                Id = this.secondMealType.Id,
                Name = this.secondMealType.Name,
                Order = this.secondMealType.Order,
            }, new MealType
            {
                Id = this.firstMealType.Id,
                Name = this.firstMealType.Name,
                Order = this.firstMealType.Order,
            });
    }

    [TestMethod]
    public async Task ShouldReturnFilteredAndSortedAndNotPagedMealTypesGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            Filter = "test",
            IsDescending = false,
            OrderBy = "order",
        };

        //WHEN
        var result = await this.service.GetMealTypesGridAsync(this.familyId, request);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(2)
            .And
            .ContainInOrder(new MealType
            {
                Id = this.firstMealType.Id,
                Name = this.firstMealType.Name,
                Order = this.firstMealType.Order,
            }, new MealType
            {
                Id = this.secondMealType.Id,
                Name = this.secondMealType.Name,
                Order = this.secondMealType.Order,
            });
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }
}
