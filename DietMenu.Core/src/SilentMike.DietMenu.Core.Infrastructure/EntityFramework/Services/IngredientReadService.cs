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

    public async Task<IngredientsGrid> GetIngredientsGridAsync(
        Guid familyId,
        GridRequest gridRequest,
        Guid? typeId,
        CancellationToken cancellationToken = default)
    {
        var filter = GetFilter(familyId, gridRequest.Filter);
        var typeFilter = GetTypeFilter(typeId);
        var orderBy = GetOrderBy(gridRequest.OrderBy);

        var query = this.context.Ingredients
            .Where(i => i.IsActive)
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

    public async Task<ReplacementsGrid> GetReplacementsGridAsync(
        Guid familyId,
        GridRequest gridRequest,
        decimal exchanger,
        decimal quantity,
        Guid typeid,
        CancellationToken cancellationToken = default)
    {
        var ingredients = await this.GetIngredientsGridAsync(familyId, gridRequest, typeid, cancellationToken);

        var replacements = new List<Replacement>();

        foreach (var ingredient in ingredients.Elements)
        {
            var replacementQuantity = exchanger == 0
                ? 0
                : Math.Round(quantity / exchanger * ingredient.Exchanger, 0);

            var replacement = new Replacement
            {
                Exchanger = ingredient.Exchanger,
                Id = ingredient.Id,
                Name = ingredient.Name,
                Quantity = replacementQuantity,
                UnitSymbol = ingredient.UnitSymbol,
            };


            replacements.Add(replacement);
        }

        var result = new ReplacementsGrid
        {
            Count = ingredients.Count,
            Elements = replacements.AsReadOnly(),
        };

        return result;
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
            "exchanger" => entity => entity.Exchanger,
            "name" => entity => entity.Name,
            "type_name" => entity => entity.Type.Name,
            "unit_symbol" => entity => entity.UnitSymbol,
            _ => entity => entity.Name,
        };
    }
}
