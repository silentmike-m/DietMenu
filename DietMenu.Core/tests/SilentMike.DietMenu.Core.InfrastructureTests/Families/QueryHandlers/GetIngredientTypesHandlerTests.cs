namespace SilentMike.DietMenu.Core.InfrastructureTests.Families.QueryHandlers;

using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Infrastructure.Families.QueryHandlers;

[TestClass]
public sealed class GetIngredientTypesHandlerTests
{
    private readonly NullLogger<GetIngredientTypesHandler> logger = new();

    [TestMethod]
    public async Task Handler_Should_Return_All_Ingredient_Types_Names()
    {
        //GIVEN
        var request = new GetIngredientTypes();

        var handler = new GetIngredientTypesHandler(this.logger);

        //WHEN
        var result = await handler.Handle(request, CancellationToken.None);

        //THEN
        result.Should()
            .HaveCount(IngredientTypeNames.IngredientTypes.Count());

        result.Should()
            .AllSatisfy(type => type.Name.Should().NotBeNullOrEmpty());

        IngredientTypeNames.IngredientTypes.Should()
            .AllSatisfy(type =>
            {
                result.Should()
                    .Contain(resultType => resultType.Type == type);
            });
    }
}
