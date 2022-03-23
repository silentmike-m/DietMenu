namespace SilentMike.DietMenu.Core.Infrastructure.Dapper.Services;

using global::AutoMapper;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;
using SilentMike.DietMenu.Core.Infrastructure.Dapper.Interfaces;

internal sealed class IngredientReadService : IIngredientReadService
{
    private const string GET_ALL_QUERY = @"SELECT * FROM SELECT *
FROM SilentMike.Ingredients I
JOIN SilentMike.IngredientTypes IT ON IT.Id = I.TypeId";

    private const string GET_ALL_COUNT_QUERY = @"SELECT COUNT(1) SELECT *
    FROM SilentMike.Ingredients I
        JOIN SilentMike.IngredientTypes IT ON IT.Id = I.TypeId";

    private readonly IMapper mapper;
    private readonly DapperOptions options;

    public IngredientReadService(IMapper mapper, IOptions<DapperOptions> options)
        => (this.mapper, this.options) = (mapper, options.Value);

    public Task<IngredientsGrid> GetIngredientsGridAsync(Guid familyId, GridRequest gridRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private static string GetOrderBy(string orderBy)
    {
        return orderBy.ToLower() switch
        {
            "exchanger" => nameof(Ingredient.Exchanger),
            "type_name" => "TypeName",
            "unit_symbol" => nameof(Ingredient.UnitSymbol),
            _ => nameof(Ingredient.Name),
        };
    }
}
