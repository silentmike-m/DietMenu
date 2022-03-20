namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class MealTypeRepository : IMealTypeRepository
{
    private readonly DietMenuDbContext context;

    public MealTypeRepository(DietMenuDbContext context) => (this.context) = (context);

    public async Task<MealTypeEntity?> Get(Guid familyId, Guid mealTypeId, CancellationToken cancellationToken = default)
    {
        return await this.context.MealTypes
            .SingleOrDefaultAsync(i => i.Id == mealTypeId, cancellationToken);
    }
}
