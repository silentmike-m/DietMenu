namespace SilentMike.DietMenu.Core.UnitTests.EntityFramework;

using System.Threading;
using AutoMapper;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.AutoMapper;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class IngredientReadServiceTests : IDisposable
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly IngredientEntity ingredientOne;
    private readonly IngredientEntity ingredientTwo;
    private readonly Guid ingredientTypeOneId = Guid.NewGuid();
    private readonly Guid ingredientTypeTwoId = Guid.NewGuid();

    private readonly DietMenuDbContextFactory factory;
    private readonly IngredientReadService service;

    public IngredientReadServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<IngredientProfile>());
        var mapper = config.CreateMapper();

        var family = new FamilyEntity(this.familyId);

        var ingredientTypeOne = new IngredientTypeEntity(this.ingredientTypeOneId)
        {
            FamilyId = this.familyId,
            InternalName = "type one",
            Name = "type one",
        };

        var ingredientTypeTwo = new IngredientTypeEntity(this.ingredientTypeTwoId)
        {
            FamilyId = this.familyId,
            InternalName = "type two",
            Name = "type two",
        };

        this.ingredientOne = new IngredientEntity(Guid.NewGuid())
        {
            Exchanger = 0.5m,
            FamilyId = this.familyId,
            InternalName = "ingredient one",
            Name = "ingredient one",
            Type = ingredientTypeOne,
            TypeId = ingredientTypeOne.Id,
            UnitSymbol = "szt.",
        };

        this.ingredientTwo = new IngredientEntity(Guid.NewGuid())
        {
            Exchanger = 2.3m,
            FamilyId = this.familyId,
            InternalName = "ingredient two",
            Name = "ingredient two",
            Type = ingredientTypeTwo,
            TypeId = ingredientTypeTwo.Id,
            UnitSymbol = "kg.",
        };

        this.factory = new DietMenuDbContextFactory(family, ingredientTypeOne, ingredientTypeTwo, ingredientOne, ingredientTwo);

        this.service = new IngredientReadService(this.factory.Context, mapper);
    }

    [TestMethod]
    public async Task ShouldReturnPagedIngredientsGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsPaged = true,
            PageNumber = 0,
            PageSize = 1,
        };

        //WHEN
        var result = await this.service.GetIngredientsGridAsync(this.familyId, request, null);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Exchanger == this.ingredientOne.Exchanger
                && i.Id == this.ingredientOne.Id
                && i.Name == this.ingredientOne.Name
                && i.TypeId == this.ingredientOne.TypeId
                && i.TypeName == this.ingredientOne.Type.Name
                && i.UnitSymbol == this.ingredientOne.UnitSymbol)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnSortedAndPagedIngredientsGrid()
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
        var result = await this.service.GetIngredientsGridAsync(this.familyId, request, null);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Exchanger == this.ingredientTwo.Exchanger
                && i.Id == this.ingredientTwo.Id
                && i.Name == this.ingredientTwo.Name
                && i.TypeId == this.ingredientTwo.TypeId
                && i.TypeName == this.ingredientTwo.Type.Name
                && i.UnitSymbol == this.ingredientTwo.UnitSymbol)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnFilteredAndSortedAndPagedIngredientsGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            Filter = this.ingredientTwo.Name,
            IsDescending = true,
            IsPaged = true,
            PageNumber = 0,
            PageSize = 1,
            OrderBy = "name",
        };

        //WHEN
        var result = await this.service.GetIngredientsGridAsync(this.familyId, request, null);

        //THEN
        result.Count.Should()
            .Be(1)
            ;
        result.Elements.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Exchanger == this.ingredientTwo.Exchanger
                && i.Id == this.ingredientTwo.Id
                && i.Name == this.ingredientTwo.Name
                && i.TypeId == this.ingredientTwo.TypeId
                && i.TypeName == this.ingredientTwo.Type.Name
                && i.UnitSymbol == this.ingredientTwo.UnitSymbol)
            ;
    }
    [TestMethod]
    public async Task ShouldReturnNotPagedIngredientsGrid()
    {
        //GIVEN
        var request = new GridRequest();

        //WHEN
        var result = await this.service.GetIngredientsGridAsync(this.familyId, request, null);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(2)
            .And
            .Contain(i =>
                i.Exchanger == this.ingredientOne.Exchanger
                && i.Id == this.ingredientOne.Id
                && i.Name == this.ingredientOne.Name
                && i.TypeId == this.ingredientOne.TypeId
                && i.TypeName == this.ingredientOne.Type.Name
                && i.UnitSymbol == this.ingredientOne.UnitSymbol)
            .And
            .Contain(i =>
                i.Exchanger == this.ingredientTwo.Exchanger
                && i.Id == this.ingredientTwo.Id
                && i.Name == this.ingredientTwo.Name
                && i.TypeId == this.ingredientTwo.TypeId
                && i.TypeName == this.ingredientTwo.Type.Name
                && i.UnitSymbol == this.ingredientTwo.UnitSymbol)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnSortedNotPagedIngredientsGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            IsDescending = true,
            OrderBy = "order",
        };

        //WHEN
        var result = await this.service.GetIngredientsGridAsync(this.familyId, request, null);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(2)
            .And
            .ContainInOrder(new Ingredient
            {
                Exchanger = this.ingredientTwo.Exchanger,
                Id = this.ingredientTwo.Id,
                Name = this.ingredientTwo.Name,
                TypeId = this.ingredientTwo.TypeId,
                TypeName = this.ingredientTwo.Type.Name,
                UnitSymbol = this.ingredientTwo.UnitSymbol,
            }, new Ingredient
            {
                Exchanger = this.ingredientOne.Exchanger,
                Id = this.ingredientOne.Id,
                Name = this.ingredientOne.Name,
                TypeId = this.ingredientOne.TypeId,
                TypeName = this.ingredientOne.Type.Name,
                UnitSymbol = this.ingredientOne.UnitSymbol,
            });
    }

    [TestMethod]
    public async Task ShouldReturnFilteredAndSortedAndNotPagedIngredientsGrid()
    {
        //GIVEN
        var request = new GridRequest
        {
            Filter = "ingredient",
            IsDescending = false,
            OrderBy = "name",
        };

        //WHEN
        var result = await this.service.GetIngredientsGridAsync(this.familyId, request, null);

        //THEN
        result.Count.Should()
            .Be(2)
            ;
        result.Elements.Should()
            .HaveCount(2)
            .And
            .ContainInOrder(new Ingredient
            {
                Exchanger = this.ingredientOne.Exchanger,
                Id = this.ingredientOne.Id,
                Name = this.ingredientOne.Name,
                TypeId = this.ingredientOne.TypeId,
                TypeName = this.ingredientOne.Type.Name,
                UnitSymbol = this.ingredientOne.UnitSymbol,
            }, new Ingredient
            {
                Exchanger = this.ingredientTwo.Exchanger,
                Id = this.ingredientTwo.Id,
                Name = this.ingredientTwo.Name,
                TypeId = this.ingredientTwo.TypeId,
                TypeName = this.ingredientTwo.Type.Name,
                UnitSymbol = this.ingredientTwo.UnitSymbol,
            });
    }

    [TestMethod]
    public async Task ShouldReturnFilteredByTypeIdIngredientsGrid()
    {
        //GIVEN
        var request = new GridRequest();

        //WHEN
        var result = await this.service.GetIngredientsGridAsync(this.familyId, request, this.ingredientTypeOneId);

        //THEN
        result.Count.Should()
            .Be(1)
            ;
        result.Elements.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Exchanger == this.ingredientOne.Exchanger
                && i.Id == this.ingredientOne.Id
                && i.Name == this.ingredientOne.Name
                && i.TypeId == this.ingredientOne.TypeId
                && i.TypeName == this.ingredientOne.Type.Name
                && i.UnitSymbol == this.ingredientOne.UnitSymbol)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnReplacementsGrid()
    {
        //GIVEN
        var exchanger = 2.5m;
        var request = new GridRequest();
        var typeId = this.ingredientTypeOneId;
        var quantity = 12m;

        //WHEN
        var result = await this.service
            .GetReplacementsGridAsync(this.familyId, request, exchanger, quantity, typeId, CancellationToken.None);


        //THEN
        result.Count.Should()
            .Be(1)
            ;

        var replacementQuantity = exchanger == 0
            ? 0
            : Math.Round(quantity / exchanger * this.ingredientOne.Exchanger, 0);

        result.Elements.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.Exchanger == this.ingredientOne.Exchanger
                && i.Id == this.ingredientOne.Id
                && i.Name == this.ingredientOne.Name
                && i.Quantity == replacementQuantity
                && i.UnitSymbol == this.ingredientOne.UnitSymbol)
            ;
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }
}
