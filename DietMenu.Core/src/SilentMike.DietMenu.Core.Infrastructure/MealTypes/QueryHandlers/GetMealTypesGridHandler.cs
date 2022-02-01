namespace SilentMike.DietMenu.Core.Infrastructure.MealTypes.QueryHandlers;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.MealTypes.Queries;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

internal sealed class GetMealTypesGridHandler : IRequestHandler<GetMealTypesGrid, MealTypesGrid>
{
    private readonly ILogger<GetMealTypesGridHandler> logger;
    private readonly IMealTypeReadService service;

    public GetMealTypesGridHandler(ILogger<GetMealTypesGridHandler> logger, IMealTypeReadService service)
        => (this.logger, this.service) = (logger, service);

    public async Task<MealTypesGrid> Handle(GetMealTypesGrid request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId)
        );

        this.logger.LogInformation("Try to get meal types grid");

        var result = await this.service.GetMealTypesGrid(request.FamilyId, request.GridRequest);

        return result;
    }
}
