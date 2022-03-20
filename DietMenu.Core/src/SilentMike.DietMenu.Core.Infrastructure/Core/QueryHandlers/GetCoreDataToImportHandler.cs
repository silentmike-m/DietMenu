namespace SilentMike.DietMenu.Core.Infrastructure.Core.QueryHandlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Core.Models;
using SilentMike.DietMenu.Core.Application.Core.Queries;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework;

internal sealed class GetCoreDataToImportHandler : IRequestHandler<GetCoreDataToImport, CoreDataToImport>
{
    private readonly DietMenuDbContext context;
    private readonly ILogger<GetCoreDataToImportHandler> logger;

    public GetCoreDataToImportHandler(DietMenuDbContext context, ILogger<GetCoreDataToImportHandler> logger)
        => (this.context, this.logger) = (context, logger);

    public async Task<CoreDataToImport> Handle(GetCoreDataToImport request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get core data to import");

        var core = await this.GetCore(cancellationToken);

        var ingredients = await this.context.CoreIngredients
            .ToListAsync(cancellationToken);

        var ingredientTypes = await this.context.CoreIngredientTypes
            .ToListAsync(cancellationToken);

        var mealTypes = await this.context.CoreMealTypes
            .ToListAsync(cancellationToken);

        var result = new CoreDataToImport
        {
            Core = core,
            Ingredients = ingredients,
            IngredientTypes = ingredientTypes,
            MealTypes = mealTypes,
        };

        return result;
    }

    private async Task<CoreEntity> GetCore(CancellationToken cancellationToken)
    {
        var core = await this.context.Core
            .SingleOrDefaultAsync(cancellationToken);

        core ??= new CoreEntity(Guid.NewGuid());

        return core;
    }
}
