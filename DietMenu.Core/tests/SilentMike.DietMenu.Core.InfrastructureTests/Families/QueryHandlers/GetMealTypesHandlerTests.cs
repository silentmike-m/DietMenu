namespace SilentMike.DietMenu.Core.InfrastructureTests.Families.QueryHandlers;

using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Infrastructure.Families.QueryHandlers;

[TestClass]
public sealed class GetMealTypesHandlerTests
{
    private readonly NullLogger<GetMealTypesHandler> logger = new();

    [TestMethod]
    public async Task Handler_Should_Return_All_Meal_Types_Names()
    {
        //GIVEN
        var request = new GetMealTypes();

        var handler = new GetMealTypesHandler(this.logger);

        //WHEN
        var result = await handler.Handle(request, CancellationToken.None);

        //THEN
        result.Should()
            .HaveCount(MealTypeNames.MealTypes.Count());

        result.Should()
            .AllSatisfy(type => type.Name.Should().NotBeNullOrEmpty());

        MealTypeNames.MealTypes.Should()
            .AllSatisfy(type =>
            {
                result.Should()
                    .Contain(resultType => resultType.Type == type);
            });
    }
}
