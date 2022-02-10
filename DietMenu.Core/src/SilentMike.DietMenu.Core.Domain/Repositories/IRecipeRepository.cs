namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IRecipeRepository
{
    Task Delete(RecipeEntity recipe, CancellationToken cancellationToken = default);
    Task<RecipeEntity?> Get(Guid recipeId, CancellationToken cancellationToken = default);
    Task Save(RecipeEntity recipe, CancellationToken cancellationToken = default);
}
