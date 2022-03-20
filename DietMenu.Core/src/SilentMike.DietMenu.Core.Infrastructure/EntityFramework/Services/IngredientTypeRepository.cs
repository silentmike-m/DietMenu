namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class IngredientTypeRepository : IIngredientTypeRepository
{
    private readonly DietMenuDbContext context;

    public IngredientTypeRepository(DietMenuDbContext context) => (this.context) = (context);

    public async Task<IngredientTypeEntity?> Get(Guid familyId, Guid ingredientTypeId, CancellationToken cancellationToken = default)
    {
        return await this.context.IngredientTypes
            .SingleOrDefaultAsync(i => i.Id == ingredientTypeId, cancellationToken);
    }
}
