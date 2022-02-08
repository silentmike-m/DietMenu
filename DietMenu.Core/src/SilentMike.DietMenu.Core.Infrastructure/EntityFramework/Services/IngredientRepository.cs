namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class IngredientRepository : IIngredientRepository
{
    private readonly DietMenuDbContext context;

    public IngredientRepository(DietMenuDbContext context) => (this.context) = (context);

    public async Task<IngredientEntity?> Get(Guid ingredientId, CancellationToken cancellationToken = default)
    {
        return await this.context.Ingredients
            .SingleOrDefaultAsync(i => i.Id == ingredientId, cancellationToken);
    }

    public async Task<IEnumerable<IngredientEntity>> GetByFamilyId(Guid familyId, CancellationToken cancellationToken = default)
    {
        var result = this.context.Ingredients.Where(i => i.FamilyId == familyId);

        return await Task.FromResult(result);
    }

    public async Task Save(IEnumerable<IngredientEntity> ingredients, CancellationToken cancellationToken = default)
    {
        await this.context.Save(ingredients, cancellationToken);
    }

    public async Task Save(IngredientEntity ingredient, CancellationToken cancellationToken = default)
    {
        await this.context.Save(ingredient, cancellationToken);
    }
}
