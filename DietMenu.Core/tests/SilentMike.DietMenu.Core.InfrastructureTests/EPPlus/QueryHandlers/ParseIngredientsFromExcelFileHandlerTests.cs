namespace SilentMike.DietMenu.Core.InfrastructureTests.EPPlus.QueryHandlers;

using SilentMike.DietMenu.Core.Application.Ingredients.Models;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Infrastructure.EPPlus.QueryHandlers;
using SilentMike.DietMenu.Core.Infrastructure.Exceptions.EPPlus;
using ErrorCodes = SilentMike.DietMenu.Core.Infrastructure.Common.Constants.ErrorCodes;

[TestClass]
public sealed class ParseIngredientsFromExcelFileHandlerTests
{
    private const string RESOURCE_NAME = "Ingredients.xlsx";

    [TestMethod]
    public async Task Should_Parse_Ingredients()
    {
        var payload = await File.ReadAllBytesAsync(RESOURCE_NAME);

        var request = new ParseIngredientsFromExcelFile
        {
            IngredientType = IngredientTypeNames.ComplexCarbohydrate,
            Payload = payload,
        };

        var handler = new ParseIngredientsFromExcelFileHandler();

        //WHEN
        var result = await handler.Handle(request, CancellationToken.None);

        //THEN
        var expectedResult = new List<IngredientToImport>
        {
            new()
            {
                Exchanger = 1.0,
                Id = new Guid("46E7473C-9072-9CFD-3ED7-485AF9998E99"),
                Name = "Amarantus",
                UnitSymbol = "g",
            },
            new()
            {
                Exchanger = 4.1,
                Id = new Guid("5CB3DB36-8733-1EBE-56AF-E7CD359A1499"),
                Name = "Bataty",
                UnitSymbol = "g",
            },
        };

        result.Should()
            .BeEquivalentTo(expectedResult)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Worksheet_Not_Found_Exception_When_Missing_Ingredient_Type_Worksheet()
    {
        var payload = await File.ReadAllBytesAsync(RESOURCE_NAME);

        var request = new ParseIngredientsFromExcelFile
        {
            IngredientType = "fail",
            Payload = payload,
        };

        var handler = new ParseIngredientsFromExcelFileHandler();

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<WorksheetNotFoundException>()
                .WithMessage($"*{request.IngredientType}*")
                .Where(exception => exception.Code == ErrorCodes.WORKSHEET_NOT_FOUND)
            ;
    }
}
