namespace SilentMike.DietMenu.Core.UnitTests.EntityFramework;

using System.Threading;
using AutoMapper;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;
using SilentMike.DietMenu.Core.Infrastructure.AutoMapper;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class IngredientReadServiceTests : IDisposable
{
    private const string INGREDIENT_TYPE_ONE_NAME = "type one";
    private const string INGREDIENT_TYPE_TWO_NAME = "type two";

    private readonly Guid familyId = Guid.NewGuid();
    private readonly IngredientRow ingredientOne;
    private readonly IngredientRow ingredientTwo;
    private readonly Guid ingredientTypeOneId = Guid.NewGuid();
    private readonly Guid ingredientTypeTwoId = Guid.NewGuid();


    private readonly DietMenuDbContextFactory factory;
    private readonly IngredientReadService service;

    public IngredientReadServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<IngredientRowProfile>());
        var mapper = config.CreateMapper();

        this.ingredientOne = new()
        {
            Exchanger = 0.5m,
            FamilyId = this.familyId,
            Id = Guid.NewGuid(),
            IsActive = true,
            Name = "ingredient one",
            TypeId = this.ingredientTypeOneId,
            TypeName = INGREDIENT_TYPE_ONE_NAME,
            UnitSymbol = "szt.",
        };

        this.ingredientTwo = new()
        {
            Exchanger = 2.3m,
            FamilyId = this.familyId,
            Id = Guid.NewGuid(),
            IsActive = true,
            Name = "ingredient two",
            TypeId = this.ingredientTypeTwoId,
            TypeName = INGREDIENT_TYPE_TWO_NAME,
            UnitSymbol = "kg.",
        };

        this.factory = new DietMenuDbContextFactory(ingredientOne, ingredientTwo);

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
                && i.TypeName == this.ingredientOne.TypeName
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
                && i.TypeName == this.ingredientTwo.TypeName
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
                && i.TypeName == this.ingredientTwo.TypeName
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
                && i.TypeName == this.ingredientOne.TypeName
                && i.UnitSymbol == this.ingredientOne.UnitSymbol)
            .And
            .Contain(i =>
                i.Exchanger == this.ingredientTwo.Exchanger
                && i.Id == this.ingredientTwo.Id
                && i.Name == this.ingredientTwo.Name
                && i.TypeId == this.ingredientTwo.TypeId
                && i.TypeName == this.ingredientTwo.TypeName
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
                TypeName = this.ingredientTwo.TypeName,
                UnitSymbol = this.ingredientTwo.UnitSymbol,
            }, new Ingredient
            {
                Exchanger = this.ingredientOne.Exchanger,
                Id = this.ingredientOne.Id,
                Name = this.ingredientOne.Name,
                TypeId = this.ingredientOne.TypeId,
                TypeName = this.ingredientOne.TypeName,
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
                TypeName = this.ingredientOne.TypeName,
                UnitSymbol = this.ingredientOne.UnitSymbol,
            }, new Ingredient
            {
                Exchanger = this.ingredientTwo.Exchanger,
                Id = this.ingredientTwo.Id,
                Name = this.ingredientTwo.Name,
                TypeId = this.ingredientTwo.TypeId,
                TypeName = this.ingredientTwo.TypeName,
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
                && i.TypeName == this.ingredientOne.TypeName
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
