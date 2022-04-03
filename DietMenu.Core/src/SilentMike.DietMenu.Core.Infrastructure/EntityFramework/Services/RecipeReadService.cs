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
            .Where(i => i.IsActive)
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
            "carbohydrates" => entity => entity.Carbohydrates,
            "energy" => entity => entity.Energy,
            "fat" => entity => entity.Fat,
            "name" => entity => entity.Name,
            "meal_type_name" => entity => entity.MealType.Name,
            "protein" => entity => entity.Protein,
            _ => entity => entity.Name,
        };
    }
}
