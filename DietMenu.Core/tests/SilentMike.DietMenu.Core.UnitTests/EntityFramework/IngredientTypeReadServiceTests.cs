namespace SilentMike.DietMenu.Core.UnitTests.EntityFramework;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.AutoMapper;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services.DbSetMoq;

[TestClass]
public sealed class IngredientTypeReadServiceTests
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly IngredientTypeEntity firstType;
    private readonly IngredientTypeEntity secondType;

    private readonly IngredientTypeReadService service;

    public IngredientTypeReadServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<IngredientTypeProfile>());
        var mapper = config.CreateMapper();

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

        var types = new List<IngredientTypeEntity>
        {
            this.firstType,
            this.secondType,
        };

        var context = new Mock<IDietMenuDbContext>();
        context.Setup(i => i.IngredientTypes).ReturnsDbSet(types);

        this.service = new IngredientTypeReadService(context.Object, mapper);
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
        var result = await this.service.GetIngredientTypesGrid(this.familyId, request);

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
        var result = await this.service.GetIngredientTypesGrid(this.familyId, request);

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
    public async Task ShouldReturnFilteredAndSortedAndPagedResult()
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
        var result = await this.service.GetIngredientTypesGrid(this.familyId, request);

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
    public async Task ShouldReturnNotPagedResult()
    {
        //GIVEN
        var request = new GridRequest();

        //WHEN
        var result = await this.service.GetIngredientTypesGrid(this.familyId, request);

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
    public async Task ShouldReturnSortedNotPagedResult()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsDescending = true,
            OrderBy = "order",
        };

        //WHEN
        var result = await this.service.GetIngredientTypesGrid(this.familyId, request);

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
        var result = await this.service.GetIngredientTypesGrid(this.familyId, request);

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
}
