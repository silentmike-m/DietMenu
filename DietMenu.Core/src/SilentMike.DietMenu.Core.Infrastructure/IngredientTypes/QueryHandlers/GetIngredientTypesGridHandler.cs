namespace SilentMike.DietMenu.Core.Infrastructure.IngredientTypes.QueryHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Queries;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

internal sealed class GetIngredientTypesGridHandler : IRequestHandler<GetIngredientTypesGrid, IngredientTypesGrid>
{
    private readonly ILogger<GetIngredientTypesGridHandler> logger;
    private readonly IIngredientTypeReadService service;

    public GetIngredientTypesGridHandler(ILogger<GetIngredientTypesGridHandler> logger, IIngredientTypeReadService service)
        => (this.logger, this.service) = (logger, service);

    public async Task<IngredientTypesGrid> Handle(GetIngredientTypesGrid request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId)
        );

        this.logger.LogInformation("Try to get ingredient types grid");

        var result = await this.service.GetIngredientTypesGrid(request.FamilyId, request.GridRequest);

        return result;
    }
}
