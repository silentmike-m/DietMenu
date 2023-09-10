namespace SilentMike.DietMenu.Core.Infrastructure.Families.QueryHandlers;

using SilentMike.DietMenu.Core.Application.Common.Extensions;
using SilentMike.DietMenu.Core.Application.Families.Models;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Domain.Common.Constants;

internal sealed class GetIngredientTypesHandler : IRequestHandler<GetIngredientTypes, IReadOnlyList<IngredientType>>
{
    private readonly ILogger<GetIngredientTypesHandler> logger;

    public GetIngredientTypesHandler(ILogger<GetIngredientTypesHandler> logger)
        => this.logger = logger;

    public async Task<IReadOnlyList<IngredientType>> Handle(GetIngredientTypes request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope("FamilyId", request.FamilyId);

        this.logger.LogInformation("Try to get ingredient types");

        var result = new List<IngredientType>
            {
                new("Węglowodan złożony", IngredientTypeNames.ComplexCarbohydrate),
                new("Owoc", IngredientTypeNames.Fruit),
                new("Zdrowy tłuszcz", IngredientTypeNames.HealthyFat),
                new("Białko wysokotłuszczowe", IngredientTypeNames.HighFatProtein),
                new("Białko niskotłuszczowe", IngredientTypeNames.LowFatProtein),
                new("Białko średniotłuszczowe", IngredientTypeNames.MediumFatProtein),
                new("Inne", IngredientTypeNames.Other),
            }
            .OrderBy(type => type.Name)
            .ToList();

        return await Task.FromResult(result);
    }
}
