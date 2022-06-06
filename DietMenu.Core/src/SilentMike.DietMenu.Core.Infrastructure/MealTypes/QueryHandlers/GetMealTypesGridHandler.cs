namespace SilentMike.DietMenu.Core.Infrastructure.MealTypes.QueryHandlers;

using SilentMike.DietMenu.Core.Application.Extensions;
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
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId)
        );

        this.logger.LogInformation("Try to get meal types grid");

        var result = await this.service.GetMealTypesGridAsync(request.FamilyId, request.GridRequest, cancellationToken);

        return result;
    }
}
