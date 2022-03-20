namespace SilentMike.DietMenu.Core.Infrastructure.Recipes.QueryHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Recipes.Queries;
using SilentMike.DietMenu.Core.Application.Recipes.ViewModels;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

internal sealed class GetRecipesGridHandler : IRequestHandler<GetRecipesGrid, RecipesGrid>
{
    private readonly ILogger<GetRecipesGridHandler> logger;
    private readonly IRecipeReadService service;

    public GetRecipesGridHandler(ILogger<GetRecipesGridHandler> logger, IRecipeReadService service)
        => (this.logger, this.service) = (logger, service);

    public async Task<RecipesGrid> Handle(GetRecipesGrid request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId)
        );

        this.logger.LogInformation("Try to get recipes grid");

        var result = await this.service.GetRecipesGrid(request.GridRequest, request.IngredientFilter, request.MealTypeId, request.UserId);

        return result;
    }
}
