namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class CoreRepository : ICoreRepository
{
    private readonly DietMenuDbContext context;

    public CoreRepository(DietMenuDbContext context) => (this.context) = (context);

    public async Task SaveIngredientsAsync(IEnumerable<CoreIngredientEntity> ingredients, CancellationToken cancellationToken = default)
    {
        await this.context.Save(ingredients, cancellationToken);
    }

    public async Task SaveIngredientTypesAsync(IEnumerable<CoreIngredientTypeEntity> ingredientTypes, CancellationToken cancellationToken = default)
    {
        await this.context.Save(ingredientTypes, cancellationToken);
    }

    public async Task SaveMealTypesAsync(IEnumerable<CoreMealTypeEntity> mealTypes, CancellationToken cancellationToken = default)
    {
        await this.context.Save(mealTypes, cancellationToken);
    }

    public async Task SaveCoreAsync(CoreEntity core, CancellationToken cancellationToken = default)
    {
        await this.context.Save(core, cancellationToken);
    }
}
