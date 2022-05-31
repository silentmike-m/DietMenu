namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class RecipeRepository : IRecipeRepository
{
    private readonly DietMenuDbContext context;

    public RecipeRepository(DietMenuDbContext context) => (this.context) = (context);

    public RecipeEntity? Get(Guid familyId, Guid recipeId)
        => this.context.Recipes
            .Include(recipe => recipe.Ingredients)
            .Where(recipe => recipe.FamilyId == familyId)
            .SingleOrDefault(recipe => recipe.Id == recipeId);


    public void Save(RecipeEntity recipe)
    {
        var track = this.context.Entry(recipe);

        switch (track.State)
        {
            case EntityState.Added or EntityState.Detached:
                this.context.Add(recipe);
                this.context.SaveChanges();
                break;
            case EntityState.Modified:
                this.context.Update(recipe);

                var existingIngredients = this.context.RecipeIngredients
                    .Where(ingredient => ingredient.RecipeId == recipe.Id)
                    .ToList();

                foreach (var existingIngredient in existingIngredients)
                {
                    var recipeIngredient = recipe.Ingredients
                        .SingleOrDefault(selectedIngredient => selectedIngredient.Id == existingIngredient.Id);

                    if (recipeIngredient is null)
                    {
                        this.context.RecipeIngredients.Remove(existingIngredient);
                    }
                }

                foreach (var recipeIngredient in recipe.Ingredients)
                {
                    var existingIngredient = existingIngredients
                        .SingleOrDefault(selectedIngredient => selectedIngredient.Id == recipeIngredient.Id);

                    if (existingIngredient is null)
                    {
                        this.context.RecipeIngredients.Add(recipeIngredient);
                    }
                    else
                    {
                        this.context.RecipeIngredients.Update(recipeIngredient);
                    }
                }

                this.context.SaveChanges();

                break;
        }
    }
}
