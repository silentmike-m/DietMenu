namespace SilentMike.DietMenu.Core.Infrastructure.Ingredients.QueryHandlers;

using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

internal sealed class GetIngredientsGridHandler : IRequestHandler<GetIngredientsGrid, IngredientsGrid>
{
    private readonly ILogger<GetIngredientsGridHandler> logger;
    private readonly IIngredientReadService service;

    public GetIngredientsGridHandler(ILogger<GetIngredientsGridHandler> logger, IIngredientReadService service)
        => (this.logger, this.service) = (logger, service);

    public async Task<IngredientsGrid> Handle(GetIngredientsGrid request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId)
        );

        this.logger.LogInformation("Try to get ingredients grid");

        var result = await this.service.GetIngredientsGridAsync(request.FamilyId, request.GridRequest, request.TypeId, cancellationToken);

        return result;
    }
}
