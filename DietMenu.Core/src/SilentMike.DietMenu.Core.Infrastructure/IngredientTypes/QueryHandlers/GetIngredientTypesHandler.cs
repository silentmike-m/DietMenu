namespace SilentMike.DietMenu.Core.Infrastructure.IngredientTypes.QueryHandlers;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Queries;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

internal sealed class GetIngredientTypesHandler : IRequestHandler<GetIngredientTypes, IngredientTypes>
{
    private readonly ILogger<GetIngredientTypesHandler> logger;
    private readonly IIngredientTypeReadService service;

    public GetIngredientTypesHandler(ILogger<GetIngredientTypesHandler> logger, IIngredientTypeReadService service)
        => (this.logger, this.service) = (logger, service);


    public async Task<IngredientTypes> Handle(GetIngredientTypes request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId)
        );

        this.logger.LogInformation("Try to get ingredient types");

        var result = await this.service.GetIngredientTypesAsync(request.FamilyId, cancellationToken);

        return result;
    }
}
