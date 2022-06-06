namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class CoreRepository : ICoreRepository
{
    private readonly DietMenuDbContext context;

    public CoreRepository(DietMenuDbContext context) => (this.context) = (context);

    public void SaveIngredients(IEnumerable<CoreIngredientEntity> ingredients)
        => this.context.Upsert(ingredients);

    public void SaveIngredientTypes(IEnumerable<CoreIngredientTypeEntity> ingredientTypes)
        => this.context.Upsert(ingredientTypes);

    public void SaveMealTypes(IEnumerable<CoreMealTypeEntity> mealTypes)
        => this.context.Upsert(mealTypes);

    public void SaveCore(CoreEntity core)
        => this.context.Upsert(core);

    public void SaveChanges()
        => this.context.SaveChanges();
}
