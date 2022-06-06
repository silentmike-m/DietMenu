namespace SilentMike.DietMenu.Core.UnitTests.Families;

using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.Families.QueryHandlers;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class GetFamilyDataToImportHandlerTests
{
    private readonly NullLogger<GetFamilyDataToImportHandler> logger = new();

    [TestMethod]
    public async Task ShouldReturnDataOnGetPortfolioLibrariesFromCoreToImport()
    {
        //GIVEN
        var family = new FamilyEntity(Guid.NewGuid());

        var ingredientType = new IngredientTypeEntity(Guid.NewGuid())
        {
            FamilyId = family.Id,
        };

        var ingredient = new IngredientEntity(Guid.NewGuid())
        {
            FamilyId = family.Id,
            Name = "ingredient",
            InternalName = Guid.NewGuid().ToString(),
            IsSystem = false,
            TypeId = ingredientType.Id,
        };

        var systemIngredient = new IngredientEntity(Guid.NewGuid())
        {
            FamilyId = family.Id,
            Name = "system ingredient",
            InternalName = Guid.NewGuid().ToString(),
            IsSystem = true,
            TypeId = ingredientType.Id,
        };

        var systemMealType = new MealTypeEntity(Guid.NewGuid())
        {
            FamilyId = family.Id,
        };

        var contextFactory = new DietMenuDbContextFactory(family, ingredientType, ingredient, systemIngredient, systemMealType);

        var request = new GetFamilyDataToImport
        {
            Family = family,
        };

        var requestHandler = new GetFamilyDataToImportHandler(contextFactory.Context, this.logger);

        //WHEN
        var result = await requestHandler.Handle(request, CancellationToken.None);

        //THEN
        result.Exceptions.Should()
            .BeEmpty()
            ;
        result.IngredientTypes.Should()
            .HaveCount(1)
            .And
            .Contain(i => i.Id == ingredientType.Id)
            ;
        result.Ingredients.Should()
            .HaveCount(1)
            .And
            .Contain(i => i.Id == systemIngredient.Id)
            .And
            .NotContain(i => i.Id == ingredient.Id)
            ;
        result.MealTypes.Should()
            .HaveCount(1)
            .And
            .Contain(i => i.Id == systemMealType.Id)
            ;
    }
}
