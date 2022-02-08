namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class FamilyRepository : IFamilyRepository
{
    private readonly DietMenuDbContext context;

    public FamilyRepository(DietMenuDbContext context) => (this.context) = (context);

    public async Task<FamilyEntity?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await this.context.Families.SingleOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task Save(FamilyEntity family, CancellationToken cancellationToken = default)
    {
        await this.context.Save(family, cancellationToken);
    }
}
