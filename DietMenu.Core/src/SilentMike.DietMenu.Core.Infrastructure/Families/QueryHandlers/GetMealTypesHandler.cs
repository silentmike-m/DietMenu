namespace SilentMike.DietMenu.Core.Infrastructure.Families.QueryHandlers;

using SilentMike.DietMenu.Core.Application.Common.Extensions;
using SilentMike.DietMenu.Core.Application.Families.Models;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Domain.Common.Constants;

internal sealed class GetMealTypesHandler : IRequestHandler<GetMealTypes, IReadOnlyList<MealType>>
{
    private readonly ILogger<GetMealTypesHandler> logger;

    public GetMealTypesHandler(ILogger<GetMealTypesHandler> logger)
        => this.logger = logger;

    public async Task<IReadOnlyList<MealType>> Handle(GetMealTypes request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope("FamilyId", request.FamilyId);

        this.logger.LogInformation("Try to get meal types");

        var result = new List<MealType>
        {
            new("I śniadanie", MealTypeNames.FirstBreakfast),
            new("II śniadanie", MealTypeNames.SecondBreakfast),
            new("Obiad", MealTypeNames.Dinner),
            new("Kolacja", MealTypeNames.Supper),
            new("Przekąska", MealTypeNames.Snack),
            new("Deser", MealTypeNames.Dessert),
        };

        return await Task.FromResult(result);
    }
}
