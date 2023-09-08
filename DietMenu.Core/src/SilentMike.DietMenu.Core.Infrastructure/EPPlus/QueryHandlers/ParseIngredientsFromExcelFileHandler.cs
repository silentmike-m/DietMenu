namespace SilentMike.DietMenu.Core.Infrastructure.EPPlus.QueryHandlers;

using OfficeOpenXml;
using SilentMike.DietMenu.Core.Application.Ingredients.Models;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Infrastructure.EPPlus.Extensions;
using SilentMike.DietMenu.Core.Infrastructure.Exceptions.EPPlus;

internal sealed class ParseIngredientsFromExcelFileHandler : IRequestHandler<ParseIngredientsFromExcelFile, IReadOnlyList<IngredientToImport>>
{
    private const int EXCHANGER_COLUMN = 3;
    private const double EXCHANGER_DEFAULT_VALUE = 0;
    private const int ID_NAME_COLUMN = 4;
    private const int NAME_COLUMN = 1;
    private const int UNIT_SYMBOL_COLUMN = 2;

    public async Task<IReadOnlyList<IngredientToImport>> Handle(ParseIngredientsFromExcelFile request, CancellationToken cancellationToken)
    {
        var fileContent = request.Payload;

        using var stream = new MemoryStream(fileContent);
        using var package = new ExcelPackage(stream);
        var worksheet = package.Workbook.Worksheets[request.IngredientType];

        if (worksheet is null)
        {
            throw new WorksheetNotFoundException(request.IngredientType);
        }

        var ingredients = new List<IngredientToImport>();
        var currentRowIndex = 2;

        while (true)
        {
            if (worksheet.Cells[currentRowIndex, Col: 2].Value == null)
            {
                break;
            }

            var id = worksheet.Cells[currentRowIndex, ID_NAME_COLUMN].Value.ToNonEmptyGuid();
            var name = worksheet.Cells[currentRowIndex, NAME_COLUMN].Value.ToEmptyString();
            var unitSymbol = worksheet.Cells[currentRowIndex, UNIT_SYMBOL_COLUMN].Value.ToEmptyString();
            var exchanger = worksheet.Cells[currentRowIndex, EXCHANGER_COLUMN].Value.ToDouble(EXCHANGER_DEFAULT_VALUE);

            var ingredient = new IngredientToImport(exchanger, id, name, unitSymbol);

            ingredients.Add(ingredient);

            currentRowIndex++;
        }

        return await Task.FromResult(ingredients.AsReadOnly());
    }
}
