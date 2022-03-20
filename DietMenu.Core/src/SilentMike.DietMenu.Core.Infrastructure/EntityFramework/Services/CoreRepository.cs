namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class CoreRepository : ICoreRepository
{
    private readonly DietMenuDbContext context;

    public CoreRepository(DietMenuDbContext context) => (this.context) = (context);

    public async Task SaveIngredients(IEnumerable<CoreIngredientEntity> ingredients, CancellationToken cancellationToken)
    {
        await this.context.Save(ingredients, cancellationToken);
    }

    public async Task SaveIngredientTypes(IEnumerable<CoreIngredientTypeEntity> ingredientTypes, CancellationToken cancellationToken)
    {
        await this.context.Save(ingredientTypes, cancellationToken);
    }

    public async Task SaveMealTypes(IEnumerable<CoreMealTypeEntity> mealTypes, CancellationToken cancellationToken)
    {
        await this.context.Save(mealTypes, cancellationToken);
    }

    public async Task SaveCore(CoreEntity core, CancellationToken cancellationToken)
    {
        await this.context.Save(core, cancellationToken);
    }
}
