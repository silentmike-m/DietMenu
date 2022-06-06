namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class IngredientTypeRepository : IIngredientTypeRepository
{
    private readonly DietMenuDbContext context;

    public IngredientTypeRepository(DietMenuDbContext context) => (this.context) = (context);

    public IngredientTypeEntity? Get(Guid familyId, Guid ingredientTypeId)
        => this.context.IngredientTypes
            .Where(ingredientType => ingredientType.FamilyId == familyId)
            .SingleOrDefault(ingredientType => ingredientType.Id == ingredientTypeId);
}
