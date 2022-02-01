namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

internal sealed class MealTypeRepository : IMealTypeRepository
{
    private readonly IDietMenuDbContext context;

    public MealTypeRepository(IDietMenuDbContext context) => (this.context) = (context);

    public async Task<MealTypeEntity?> Get(Guid familyId, Guid mealTypeId, CancellationToken cancellationToken = default)
    {
        return await this.context.MealTypes
            .SingleOrDefaultAsync(i => i.FamilyId == familyId && i.Id == mealTypeId, cancellationToken);
    }

    public async Task<IEnumerable<MealTypeEntity>> Get(Guid familyId, CancellationToken cancellationToken = default)
    {
        var result = this.context.MealTypes.Where(i => i.FamilyId == familyId);

        return await Task.FromResult(result);
    }

    public async Task Save(MealTypeEntity mealType, CancellationToken cancellationToken = default)
    {
        await this.context.Save(mealType, cancellationToken);
    }

    public async Task Save(IEnumerable<MealTypeEntity> mealTypes, CancellationToken cancellationToken = default)
    {
        await this.context.Save(mealTypes, cancellationToken);
    }
}
