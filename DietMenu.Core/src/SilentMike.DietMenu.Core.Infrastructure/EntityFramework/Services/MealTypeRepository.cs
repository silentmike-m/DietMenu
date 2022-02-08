namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class MealTypeRepository : IMealTypeRepository
{
    private readonly DietMenuDbContext context;

    public MealTypeRepository(DietMenuDbContext context) => (this.context) = (context);

    public async Task<MealTypeEntity?> Get(Guid mealTypeId, CancellationToken cancellationToken = default)
    {
        return await this.context.MealTypes
            .SingleOrDefaultAsync(i => i.Id == mealTypeId, cancellationToken);
    }

    public async Task<IEnumerable<MealTypeEntity>> GetByFamilyId(Guid familyId, CancellationToken cancellationToken = default)
    {
        var result = this.context.MealTypes.Where(i => i.FamilyId == familyId);

        return await Task.FromResult(result);
    }

    public async Task Save(IEnumerable<MealTypeEntity> mealTypes, CancellationToken cancellationToken = default)
    {
        await this.context.Save(mealTypes, cancellationToken);
    }

    public async Task Save(MealTypeEntity mealType, CancellationToken cancellationToken = default)
    {
        await this.context.Save(mealType, cancellationToken);
    }
}
