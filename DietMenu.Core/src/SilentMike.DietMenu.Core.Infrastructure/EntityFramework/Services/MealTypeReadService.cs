namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using System.Linq.Expressions;
using global::AutoMapper;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Extensions;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

internal sealed class MealTypeReadService : IMealTypeReadService
{
    private readonly DietMenuDbContext context;
    private readonly IMapper mapper;

    public MealTypeReadService(DietMenuDbContext context, IMapper mapper)
        => (this.context, this.mapper) = (context, mapper);

    public async Task<MealTypesGrid> GetMealTypesGrid(Guid familyId, GridRequest gridRequest)
    {
        var filter = GetFilter(familyId, gridRequest.Filter);
        var orderBy = GetOrderBy(gridRequest.OrderBy);

        var types = this.context.MealTypes
            .GetFiltered(filter)
            .GetOrdered(orderBy, gridRequest.IsDescending)
            .GetPaged(gridRequest.PageNumber, gridRequest.IsPaged, gridRequest.PageSize);

        var count = this.context.MealTypes.GetItemsCount(filter);

        var mealTypes = this.mapper.Map<IReadOnlyList<MealType>>(types);

        var result = new MealTypesGrid
        {
            Count = count,
            Elements = mealTypes,
        };

        return await Task.FromResult(result);
    }

    private static Expression<Func<MealTypeEntity, bool>> GetFilter(Guid familyId, string filter)
    {
        if (string.IsNullOrEmpty(filter))
        {
            return entity => entity.FamilyId == familyId;
        }

        filter = filter.ToLower();

        return entity => entity.FamilyId == familyId && entity.Name.ToLower().Contains(filter);
    }

    private static Expression<Func<MealTypeEntity, object>> GetOrderBy(string orderBy)
    {
        return orderBy.ToLower() switch
        {
            "name" => entity => entity.Name,
            "order" => entity => entity.Order,
            _ => entity => entity.Order,
        };
    }
}
