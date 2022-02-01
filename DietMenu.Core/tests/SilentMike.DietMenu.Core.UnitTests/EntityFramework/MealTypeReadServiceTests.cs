namespace SilentMike.DietMenu.Core.UnitTests.EntityFramework;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.AutoMapper;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services.DbSetMoq;

[TestClass]
public sealed class MealTypeReadServiceTests
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly MealTypeEntity firstMealType;
    private readonly MealTypeEntity secondMealType;

    private readonly MealTypeReadService service;

    public MealTypeReadServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MealTypeProfile>());
        var mapper = config.CreateMapper();

        this.firstMealType = new(Guid.NewGuid())
        {
            FamilyId = this.familyId,
            InternalName = Guid.NewGuid().ToString(),
            Name = "test_name_1",
            Order = 1,
        };
        this.secondMealType = new(Guid.NewGuid())
        {
            FamilyId = this.familyId,
            InternalName = Guid.NewGuid().ToString(),
            Name = "test_name_2",
            Order = 2,
        };

        var mealTypes = new List<MealTypeEntity>
        {
            this.firstMealType,
            this.secondMealType,
        };

        var context = new Mock<IDietMenuDbContext>();
        context.Setup(i => i.MealTypes).ReturnsDbSet(mealTypes);

        this.service = new MealTypeReadService(context.Object, mapper);
    }

    [TestMethod]
    public async Task ShouldReturnPagedResult()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsPaged = true,
            PageNumber = 0,
            PageSize = 1,
        };

        //WHEN
        var result = await this.service.GetMealTypesGrid(this.familyId, request);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Id == this.firstMealType.Id
                && i.Name == this.firstMealType.Name
                && i.Order == this.firstMealType.Order)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnSortedAndPagedResult()
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
        var result = await this.service.GetMealTypesGrid(this.familyId, request);

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
    public async Task ShouldReturnFilteredAndSortedAndPagedResult()
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
        var result = await this.service.GetMealTypesGrid(this.familyId, request);

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
    public async Task ShouldReturnNotPagedResult()
    {
        //GIVEN
        var request = new GridRequest();

        //WHEN
        var result = await this.service.GetMealTypesGrid(this.familyId, request);

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
        var result = await this.service.GetMealTypesGrid(this.familyId, request);

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
    public async Task ShouldReturnFilteredAndSortedAndNotPagedResult()
    {
        //GIVEN
        var request = new GridRequest
        {
            Filter = "test",
            IsDescending = false,
            OrderBy = "order",
        };

        //WHEN
        var result = await this.service.GetMealTypesGrid(this.familyId, request);

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
}
