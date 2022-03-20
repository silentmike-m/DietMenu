namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IRecipeRepository
{
    Task<RecipeEntity?> Get(Guid familyId, Guid recipeId, CancellationToken cancellationToken = default);
    Task Save(RecipeEntity recipe, CancellationToken cancellationToken = default);
}
