namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class MealTypeRepository : IMealTypeRepository
{
    private readonly DietMenuDbContext context;

    public MealTypeRepository(DietMenuDbContext context) => (this.context) = (context);

    public MealTypeEntity? Get(Guid familyId, Guid mealTypeId)
        => this.context.MealTypes
            .Where(mealType => mealType.FamilyId == familyId)
            .SingleOrDefault(mealType => mealType.Id == mealTypeId);
}
