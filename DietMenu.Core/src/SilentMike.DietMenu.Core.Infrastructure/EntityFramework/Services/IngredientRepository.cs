namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class IngredientRepository : IIngredientRepository
{
    private readonly DietMenuDbContext context;

    public IngredientRepository(DietMenuDbContext context) => (this.context) = (context);

    public IngredientEntity? Get(Guid familyId, Guid ingredientId)
        => this.context.Ingredients
            .Include(ingredient => ingredient.Type)
            .Where(ingredient => ingredient.FamilyId == familyId)
            .SingleOrDefault(ingredient => ingredient.Id == ingredientId);

    public void Save(IngredientEntity ingredient)
    {
        this.context.Upsert(ingredient);

        this.context.SaveChanges();
    }
}
