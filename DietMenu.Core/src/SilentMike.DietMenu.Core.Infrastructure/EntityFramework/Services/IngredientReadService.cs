namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using System.Linq.Expressions;
using global::AutoMapper;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Extensions;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

internal sealed class IngredientReadService : IIngredientReadService
{
    private readonly DietMenuDbContext context;
    private readonly IMapper mapper;

    public IngredientReadService(DietMenuDbContext context, IMapper mapper)
        => (this.context, this.mapper) = (context, mapper);

    public async Task<IngredientsGrid> GetIngredientsGrid(Guid familyId, GridRequest gridRequest)
    {
        var filter = GetFilter(familyId, gridRequest.Filter);
        var orderBy = GetOrderBy(gridRequest.OrderBy);

        var ingredients = this.context.Ingredients
            .Include(i => i.Type)
            .GetFiltered(filter)
            .GetOrdered(orderBy, gridRequest.IsDescending)
            .GetPaged(gridRequest.PageNumber, gridRequest.IsPaged, gridRequest.PageSize)
            .ToList();

        var count = this.context.Ingredients.GetItemsCount(filter);

        var resultType = this.mapper.Map<List<Ingredient>>(ingredients);

        var result = new IngredientsGrid
        {
            Count = count,
            Elements = resultType,
        };

        return await Task.FromResult(result);
    }

    private static Expression<Func<IngredientEntity, bool>> GetFilter(Guid familyId, string filter)
    {
        if (string.IsNullOrEmpty(filter))
        {
            return entity => entity.FamilyId == familyId;
        }

        filter = filter.ToLower();

        return entity => entity.FamilyId == familyId
                         && entity.Name.ToLower().Contains(filter)
                         || entity.Type.Name.ToLower().Contains(filter);
    }

    private static Expression<Func<IngredientEntity, object>> GetOrderBy(string orderBy)
    {
        return orderBy.ToLower() switch
        {
            "exchanger" => entity => entity.Exchanger,
            "name" => entity => entity.Name,
            "type_name" => entity => entity.Type.Name,
            "unit_symbol" => entity => entity.UnitSymbol,
            _ => entity => entity.Name,
        };
    }
}
