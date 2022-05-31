namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class CoreRepository : ICoreRepository
{
    private readonly DietMenuDbContext context;

    public CoreRepository(DietMenuDbContext context) => (this.context) = (context);

    public void SaveIngredients(IEnumerable<CoreIngredientEntity> ingredients)
    {
        this.context.Upsert(ingredients);

        this.context.SaveChanges();
    }

    public void SaveIngredientTypes(IEnumerable<CoreIngredientTypeEntity> ingredientTypes)
    {
        this.context.Upsert(ingredientTypes);

        this.context.SaveChanges();
    }

    public void SaveMealTypes(IEnumerable<CoreMealTypeEntity> mealTypes)
    {
        this.context.Upsert(mealTypes);

        this.context.SaveChanges();
    }

    public void SaveCore(CoreEntity core)
    {
        this.context.Upsert(core);

        this.context.SaveChanges();
    }
}
