namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using System.Linq.Expressions;
using global::AutoMapper;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Recipes.ViewModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Extensions;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

internal sealed class RecipeReadService : IRecipeReadService
{
    private const string CARBOHYDRATES_ORDER_BY = "carbohydrates";
    private const string ENERGY_ORDER_BY = "energy";
    private const string FAT_ORDER_BY = "fat";
    private const string NAME_ORDER_BY = "name";
    private const string MEAL_TYPE_ORDER_BY = "meal_type_name";
    private const string PROTEIN_ORDER_BY = "protein";

    private readonly DietMenuDbContext context;
    private readonly IMapper mapper;

    public RecipeReadService(DietMenuDbContext context, IMapper mapper)
        => (this.context, this.mapper) = (context, mapper);

    public async Task<RecipesGrid> GetRecipesGridAsync(GridRequest gridRequest, string? ingredientsFilter, Guid? mealTypeId, Guid userId, CancellationToken cancellationToken = default)
    {
        var filter = GetFilter(gridRequest.Filter, mealTypeId, userId);
        var filterIngredients = GetIngredientsFilter(ingredientsFilter);
        var orderBy = GetOrderBy(gridRequest.OrderBy);

        var query = this.context.Recipes
            .Include(i => i.MealType)
            .Include(i => i.Ingredients)
            .ThenInclude(i => i.Ingredient)
            .ThenInclude(i => i.Type)
            .GetFiltered(filter)
            .GetFiltered(filterIngredients);

        var recipes = query
            .GetOrdered(orderBy, gridRequest.IsDescending)
            .GetPaged(gridRequest.PageNumber, gridRequest.IsPaged, gridRequest.PageSize);

        var count = query.Count();

        var recipesResult = this.mapper.Map<IReadOnlyList<Recipe>>(recipes);

        var result = new RecipesGrid
        {
            Count = count,
            Elements = recipesResult,
        };

        return await Task.FromResult(result);
    }

    private static Expression<Func<RecipeEntity, bool>> GetFilter(string filter, Guid? mealTypeId, Guid userId)
    {
        if (string.IsNullOrEmpty(filter))
        {
            return entity => entity.UserId == userId
                             && (entity.MealTypeId == mealTypeId || !mealTypeId.HasValue);
        }

        filter = filter.ToLower();

        return entity => entity.UserId == userId
                         && entity.Name.ToLower().Contains(filter)
                         && (entity.MealTypeId == mealTypeId || !mealTypeId.HasValue);
    }

    private static Expression<Func<RecipeEntity, bool>> GetIngredientsFilter(string? filter)
    {
        if (string.IsNullOrEmpty(filter))
        {
            return entity => true;
        }

        filter = filter.ToLower();

        return entity => entity.Ingredients.Any(i => i.Ingredient.Name.ToLower().Contains(filter));
    }

    private static Expression<Func<RecipeEntity, object>> GetOrderBy(string orderBy)
    {
        return orderBy.ToLower() switch
        {
            CARBOHYDRATES_ORDER_BY => entity => entity.Carbohydrates,
            ENERGY_ORDER_BY => entity => entity.Energy,
            FAT_ORDER_BY => entity => entity.Fat,
            NAME_ORDER_BY => entity => entity.Name,
            MEAL_TYPE_ORDER_BY => entity => entity.MealType.Name,
            PROTEIN_ORDER_BY => entity => entity.Protein,
            _ => entity => entity.Name,
        };
    }
}
