namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IRecipeRepository : IRepository
{
    RecipeEntity? Get(Guid familyId, Guid recipeId);
    void Save(RecipeEntity recipe);
}
