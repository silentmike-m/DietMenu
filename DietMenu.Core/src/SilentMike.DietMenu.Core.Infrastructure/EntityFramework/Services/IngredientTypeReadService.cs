namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using System.Linq.Expressions;
using global::AutoMapper;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Extensions;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models;

internal sealed class IngredientTypeReadService : IIngredientTypeReadService
{
    private readonly DietMenuDbContext context;
    private readonly IMapper mapper;

    public IngredientTypeReadService(DietMenuDbContext context, IMapper mapper)
        => (this.context, this.mapper) = (context, mapper);

    public async Task<IngredientTypes> GetIngredientTypesAsync(Guid familyId, CancellationToken cancellationToken = default)
    {
        var types = await this.context.IngredientTypeRows
            .Where(type => type.FamilyId == familyId)
            .Where(i => i.IsActive)
            .ToListAsync(cancellationToken);

        var ingredientTypes = this.mapper.Map<IReadOnlyList<IngredientType>>(types);

        var result = new IngredientTypes
        {
            Types = ingredientTypes,
        };

        return result;
    }

    public async Task<IngredientTypesGrid> GetIngredientTypesGridAsync(Guid familyId, GridRequest gridRequest, CancellationToken cancellationToken = default)
    {
        var filter = GetFilter(familyId, gridRequest.Filter);
        var orderBy = GetOrderBy();

        var types = this.context.IngredientTypeRows
            .Where(i => i.IsActive)
            .GetFiltered(filter)
            .GetOrdered(orderBy, gridRequest.IsDescending)
            .GetPaged(gridRequest.PageNumber, gridRequest.IsPaged, gridRequest.PageSize);

        var count = this.context.IngredientTypeRows.GetItemsCount(filter);

        var ingredientTypes = this.mapper.Map<IReadOnlyList<IngredientType>>(types);

        var result = new IngredientTypesGrid
        {
            Count = count,
            Elements = ingredientTypes,
        };

        return await Task.FromResult(result);
    }

    private static Expression<Func<IngredientTypeRow, bool>> GetFilter(Guid familyId, string filter)
    {
        if (string.IsNullOrEmpty(filter))
        {
            return entity => entity.FamilyId == familyId;
        }

        filter = filter.ToLower();

        return entity => entity.FamilyId == familyId && entity.Name.ToLower().Contains(filter);
    }

    private static Expression<Func<IngredientTypeRow, object>> GetOrderBy()
    {
        return entity => entity.Name;
    }
}
