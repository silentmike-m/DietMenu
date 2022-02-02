namespace SilentMike.DietMenu.Core.Infrastructure.EPPlus.QueryHandlers;

using MediatR;
using OfficeOpenXml;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EPPlus.Extensions;

internal sealed class ParseIngredientsFromExcelFileHandler : IRequestHandler<ParseIngredientsFromExcelFile, IReadOnlyList<IngredientEntity>>
{
    public async Task<IReadOnlyList<IngredientEntity>> Handle(ParseIngredientsFromExcelFile request, CancellationToken cancellationToken)
    {
        var fileContent = request.Payload;

        using var stream = new MemoryStream(fileContent);
        using var package = new ExcelPackage(stream);
        ExcelWorksheet worksheet = package.Workbook.Worksheets[request.TypeInternalName];

        var ingredients = new List<IngredientEntity>();
        var currentRowIndex = 2;

        while (true)
        {
            if (worksheet.Cells[currentRowIndex, 2].Value == null)
            {
                break;
            }

            var internalName = worksheet.Cells[currentRowIndex, 4].Value?.ToString() ?? Guid.NewGuid().ToString();
            var name = worksheet.Cells[currentRowIndex, 1].Value?.ToString() ?? string.Empty;
            var unitSymbol = worksheet.Cells[currentRowIndex, 2].Value?.ToString() ?? string.Empty;
            var exchanger = worksheet.Cells[currentRowIndex, 3].Value?.ToDecimal();

            var ingredient = new IngredientEntity(Guid.NewGuid())
            {
                Exchanger = exchanger ?? 1,
                FamilyId = request.FamilyId,
                IsSystem = true,
                InternalName = internalName,
                Name = name,
                TypeId = request.TypeId,
                UnitSymbol = unitSymbol,
            };

            ingredients.Add(ingredient);

            currentRowIndex++;
        }

        return await Task.FromResult(ingredients.AsReadOnly());
    }
}
