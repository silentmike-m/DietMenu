namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

internal sealed class FamilyRepository : IFamilyRepository
{
    private readonly IDietMenuDbContext context;

    public FamilyRepository(IDietMenuDbContext context) => (this.context) = (context);

    public async Task<FamilyEntity?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await this.context.Families.SingleOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task Save(FamilyEntity family, CancellationToken cancellationToken = default)
    {
        await this.context.Save(family, cancellationToken);
    }
}
