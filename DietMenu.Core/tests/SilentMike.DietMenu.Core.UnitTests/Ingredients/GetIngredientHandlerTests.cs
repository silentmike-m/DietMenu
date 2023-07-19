namespace SilentMike.DietMenu.Core.UnitTests.Ingredients;

using AutoMapper;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;
using SilentMike.DietMenu.Core.Infrastructure.AutoMapper;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models;
using SilentMike.DietMenu.Core.Infrastructure.Ingredients.QueryHandlers;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class GetIngredientHandlerTests : IDisposable
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly IngredientRow ingredient;

    private readonly DietMenuDbContextFactory factory;
    private readonly NullLogger<GetIngredientHandler> logger = new();
    private readonly IMapper mapper;

    public GetIngredientHandlerTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<IngredientRowProfile>());
        this.mapper = config.CreateMapper();

        this.ingredient = new()
        {
            Exchanger = 0.5m,
            FamilyId = this.familyId,
            Id = Guid.NewGuid(),
            Name = "ingredient one",
            TypeId = Guid.NewGuid(),
            TypeName = "type one",
            UnitSymbol = "szt.",
        };

        this.factory = new DietMenuDbContextFactory(ingredient);
    }

    [TestMethod]
    public async Task ShouldThrowIngredientNotFoundWhenInvalidIdOnGetIngredient()
    {
        //GIVEN
        var request = new GetIngredient
        {
            FamilyId = this.familyId,
            Id = Guid.NewGuid(),
        };

        var requestHandler = new GetIngredientHandler(this.factory.Context, this.logger, this.mapper);

        //WHEN
        Func<Task<Ingredient>> action = async () => await requestHandler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<IngredientNotFoundException>()
                .Where(i =>
                    i.Code == ErrorCodes.INGREDIENT_NOT_FOUND
                    && i.Id == request.Id)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowIngredientNotFoundWhenInvalidFamilyIdOnGetIngredient()
    {
        //GIVEN
        var request = new GetIngredient
        {
            FamilyId = Guid.NewGuid(),
            Id = this.ingredient.Id,
        };

        var requestHandler = new GetIngredientHandler(this.factory.Context, this.logger, this.mapper);

        //WHEN
        Func<Task<Ingredient>> action = async () => await requestHandler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<IngredientNotFoundException>()
                .Where(i =>
                    i.Code == ErrorCodes.INGREDIENT_NOT_FOUND
                    && i.Id == request.Id)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnIngredientOnGetIngredient()
    {
        //GIVEN
        var request = new GetIngredient
        {
            FamilyId = this.familyId,
            Id = this.ingredient.Id,
        };

        var requestHandler = new GetIngredientHandler(this.factory.Context, this.logger, this.mapper);

        //WHEN
        var result = await requestHandler.Handle(request, CancellationToken.None);

        //THEN
        result.Should()
            .BeEquivalentTo(this.ingredient, opt => opt.ExcludingMissingMembers())
            ;
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }
}
