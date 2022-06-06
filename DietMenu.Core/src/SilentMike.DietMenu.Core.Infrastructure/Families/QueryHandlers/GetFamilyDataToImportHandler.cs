namespace SilentMike.DietMenu.Core.Infrastructure.Families.QueryHandlers;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Application.Families.Models;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework;

internal sealed class GetFamilyDataToImportHandler : IRequestHandler<GetFamilyDataToImport, FamilyDataToImport>
{
    private readonly DietMenuDbContext context;
    private readonly ILogger<GetFamilyDataToImportHandler> logger;

    public GetFamilyDataToImportHandler(DietMenuDbContext context, ILogger<GetFamilyDataToImportHandler> logger)
        => (this.context, this.logger) = (context, logger);

    public async Task<FamilyDataToImport> Handle(GetFamilyDataToImport request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get family data to import");

        var ingredients = await this.context.Ingredients
            .Where(i => i.FamilyId == request.Family.Id)
            .Where(i => i.IsSystem)
            .ToListAsync(cancellationToken);

        var ingredientTypes = await this.context.IngredientTypes
            .Where(i => i.FamilyId == request.Family.Id)
            .ToListAsync(cancellationToken);

        var mealTypes = await this.context.MealTypes
            .Where(i => i.FamilyId == request.Family.Id)
            .ToListAsync(cancellationToken);

        var result = new FamilyDataToImport
        {
            Ingredients = ingredients,
            IngredientTypes = ingredientTypes,
            MealTypes = mealTypes,
        };

        return result;
    }
}
