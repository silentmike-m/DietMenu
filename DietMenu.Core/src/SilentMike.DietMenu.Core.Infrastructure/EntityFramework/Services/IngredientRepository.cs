namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class IngredientRepository : IIngredientRepository
{
    private readonly DietMenuDbContext context;

    public IngredientRepository(DietMenuDbContext context) => (this.context) = (context);

    public async Task<IngredientEntity?> GetAsync(Guid familyId, Guid ingredientId, CancellationToken cancellationToken = default)
    {
        return await this.context.Ingredients
            .SingleOrDefaultAsync(i => i.Id == ingredientId && i.IsActive, cancellationToken);
    }

    public async Task SaveAsync(IngredientEntity ingredient, CancellationToken cancellationToken = default)
    {
        await this.context.Save(ingredient, cancellationToken);
    }
}
