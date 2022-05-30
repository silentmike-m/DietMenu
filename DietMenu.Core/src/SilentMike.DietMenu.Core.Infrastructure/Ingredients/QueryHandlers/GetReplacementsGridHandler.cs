namespace SilentMike.DietMenu.Core.Infrastructure.Ingredients.QueryHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

internal sealed class GetReplacementsGridHandler : IRequestHandler<GetReplacementsGrid, ReplacementsGrid>
{
    private readonly ILogger<GetReplacementsGridHandler> logger;
    private readonly IIngredientReadService service;

    public GetReplacementsGridHandler(ILogger<GetReplacementsGridHandler> logger, IIngredientReadService service)
        => (this.logger, this.service) = (logger, service);
    public async Task<ReplacementsGrid> Handle(GetReplacementsGrid request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId)
        );

        this.logger.LogInformation("Try to get replacements grid");

        var result = await this.service
            .GetReplacementsGridAsync(request.FamilyId, request.GridRequest, request.Exchanger, request.Quantity, request.TypeId, cancellationToken);

        return result;
    }
}
