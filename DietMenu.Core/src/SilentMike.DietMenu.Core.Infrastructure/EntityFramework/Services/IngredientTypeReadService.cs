namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using System.Linq.Expressions;
using global::AutoMapper;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Extensions;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

internal sealed class IngredientTypeReadService : IIngredientTypeReadService
{
    private readonly IDietMenuDbContext context;
    private readonly IMapper mapper;

    public IngredientTypeReadService(IDietMenuDbContext context, IMapper mapper)
        => (this.context, this.mapper) = (context, mapper);

    public async Task<IngredientTypesGrid> GetIngredientTypesGrid(Guid familyId, GridRequest gridRequest)
    {
        var filter = GetFilter(familyId, gridRequest.Filter);
        var orderBy = GetOrderBy();

        var types = this.context.IngredientTypes
            .GetFiltered(filter)
            .GetOrdered(orderBy, gridRequest.IsDescending)
            .GetPaged(gridRequest.PageNumber, gridRequest.IsPaged, gridRequest.PageSize);

        var count = this.context.IngredientTypes.GetItemsCount(filter);

        var ingredientTypes = this.mapper.Map<IReadOnlyList<IngredientType>>(types);

        var result = new IngredientTypesGrid
        {
            Count = count,
            Elements = ingredientTypes,
        };

        return await Task.FromResult(result);
    }

    private static Expression<Func<IngredientTypeEntity, bool>> GetFilter(Guid familyId, string filter)
    {
        if (string.IsNullOrEmpty(filter))
        {
            return entity => entity.FamilyId == familyId;
        }

        filter = filter.ToLower();

        return entity => entity.FamilyId == familyId && entity.Name.ToLower().Contains(filter);
    }

    private static Expression<Func<IngredientTypeEntity, object>> GetOrderBy()
    {
        return entity => entity.Name;
    }
}
