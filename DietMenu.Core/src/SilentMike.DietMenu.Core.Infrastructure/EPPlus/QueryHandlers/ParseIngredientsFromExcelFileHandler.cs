namespace SilentMike.DietMenu.Core.Infrastructure.EPPlus.QueryHandlers;

using OfficeOpenXml;
using SilentMike.DietMenu.Core.Application.Ingredients.Models;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Infrastructure.EPPlus.Extensions;
using SilentMike.DietMenu.Core.Infrastructure.Exceptions.EPPlus;

internal sealed class ParseIngredientsFromExcelFileHandler : IRequestHandler<ParseIngredientsFromExcelFile, IReadOnlyList<IngredientFromExcelFile>>
{
    private const int EXCHANGER_COLUMN = 3;
    private const decimal EXCHANGER_DEFAULT_VALUE = 0;
    private const int INTERNAL_NAME_COLUMN = 4;
    private const int NAME_COLUMN = 1;
    private const int UNIT_SYMBOL_COLUMN = 2;

    public async Task<IReadOnlyList<IngredientFromExcelFile>> Handle(ParseIngredientsFromExcelFile request, CancellationToken cancellationToken)
    {
        var fileContent = request.Payload;

        using var stream = new MemoryStream(fileContent);
        using var package = new ExcelPackage(stream);
        var worksheet = package.Workbook.Worksheets[request.IngredientType];

        if (worksheet is null)
        {
            throw new WorksheetNotFoundException(request.IngredientType);
        }

        var ingredients = new List<IngredientFromExcelFile>();
        var currentRowIndex = 2;

        while (true)
        {
            if (worksheet.Cells[currentRowIndex, Col: 2].Value == null)
            {
                break;
            }

            var internalName = worksheet.Cells[currentRowIndex, INTERNAL_NAME_COLUMN].Value?.ToString() ?? string.Empty;
            var name = worksheet.Cells[currentRowIndex, NAME_COLUMN].Value?.ToString() ?? string.Empty;
            var unitSymbol = worksheet.Cells[currentRowIndex, UNIT_SYMBOL_COLUMN].Value?.ToString() ?? string.Empty;
            var exchanger = worksheet.Cells[currentRowIndex, EXCHANGER_COLUMN].Value?.ToDecimal(EXCHANGER_DEFAULT_VALUE);

            var ingredient = new IngredientFromExcelFile
            {
                Exchanger = exchanger ?? EXCHANGER_DEFAULT_VALUE,
                InternalName = internalName,
                Name = name,
                UnitSymbol = unitSymbol,
            };

            ingredients.Add(ingredient);

            currentRowIndex++;
        }

        return await Task.FromResult(ingredients.AsReadOnly());
    }
}
