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
    private const string EXCHANGER_ORDER_BY = "exchanger";
    private const string NAME_ORDER_BY = "name";
    private const string TYPE_ORDER_BY = "type_name";
    private const string UNIT_ORDER_BY = "unit_symbol";

    private readonly DietMenuDbContext context;
    private readonly IMapper mapper;

    public IngredientReadService(DietMenuDbContext context, IMapper mapper)
        => (this.context, this.mapper) = (context, mapper);

    public async Task<IngredientsGrid> GetIngredientsGridAsync(Guid familyId, GridRequest gridRequest, Guid? typeId, CancellationToken cancellationToken = default)
    {
        var filter = GetFilter(familyId, gridRequest.Filter);
        var typeFilter = GetTypeFilter(typeId);
        var orderBy = GetOrderBy(gridRequest.OrderBy);

        var query = this.context.Ingredients
            .Include(i => i.Type)
            .GetFiltered(filter)
            .GetFiltered(typeFilter);

        var ingredients = query
            .GetOrdered(orderBy, gridRequest.IsDescending)
            .GetPaged(gridRequest.PageNumber, gridRequest.IsPaged, gridRequest.PageSize);

        var count = query.Count();

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
    private static Expression<Func<IngredientEntity, bool>> GetTypeFilter(Guid? typeId)
    {
        if (typeId is null)
        {
            return entity => true;
        }

        return entity => entity.TypeId == typeId;
    }

    private static Expression<Func<IngredientEntity, object>> GetOrderBy(string orderBy)
    {
        return orderBy.ToLower() switch
        {
            EXCHANGER_ORDER_BY => entity => entity.Exchanger,
            NAME_ORDER_BY => entity => entity.Name,
            TYPE_ORDER_BY => entity => entity.Type.Name,
            UNIT_ORDER_BY => entity => entity.UnitSymbol,
            _ => entity => entity.Name,
        };
    }
}
