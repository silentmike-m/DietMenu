namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IRecipeRepository
{
    Task<RecipeEntity?> GetAsync(Guid familyId, Guid recipeId, CancellationToken cancellationToken = default);
    Task SaveAsync(RecipeEntity recipe, CancellationToken cancellationToken = default);
}
