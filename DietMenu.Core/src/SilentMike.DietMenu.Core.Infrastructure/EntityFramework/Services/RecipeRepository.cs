namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class RecipeRepository : IRecipeRepository
{
    private readonly DietMenuDbContext context;

    public RecipeRepository(DietMenuDbContext context) => (this.context) = (context);

    public async Task Delete(RecipeEntity recipe, CancellationToken cancellationToken = default)
    {
        this.context.Remove(recipe);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task<RecipeEntity?> Get(Guid recipeId, CancellationToken cancellationToken = default)
    {
        return await this.context.Recipes
            .Include(i => i.Ingredients)
            .SingleOrDefaultAsync(i => i.Id == recipeId, cancellationToken);
    }

    public async Task Save(RecipeEntity recipe, CancellationToken cancellationToken = default)
    {
        var track = this.context.Entry(recipe);

        switch (track.State)
        {
            case EntityState.Added or EntityState.Detached:
                await this.context.AddAsync(recipe, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);
                break;
            case EntityState.Modified:
                this.context.Update(recipe);

                var existingIngredients = this.context.RecipeIngredients
                    .Where(i => i.RecipeId == recipe.Id)
                    .ToList();

                var toAdd = recipe.Ingredients
                    .Where(i => existingIngredients.All(j => j.Id != i.Id));

                var toDelete = existingIngredients
                    .Where(i => recipe.Ingredients.All(j => j.Id != i.Id));

                var toUpdate = recipe.Ingredients
                    .Where(i => existingIngredients.Any(j => j.Id == i.Id));

                await this.context.AddRangeAsync(toAdd, cancellationToken);

                this.context.RemoveRange(toDelete);

                this.context.UpdateRange(toUpdate);

                await this.context.SaveChangesAsync(cancellationToken);

                break;
        }

        await this.context.Save(recipe, cancellationToken);
    }
}
