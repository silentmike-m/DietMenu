namespace DietMenu.Core.Infrastructure.EntityFramework.Services;

using DietMenu.Core.Domain.Entities;
using DietMenu.Core.Domain.Repositories;
using DietMenu.Core.Infrastructure.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

internal sealed class FamilyRepository : IFamilyRepository
{
    private readonly IApplicationDbContext context;

    public FamilyRepository(IApplicationDbContext context)
        => (this.context) = (context);

    public async Task<FamilyEntity?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await this.context.Families.SingleOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<FamilyEntity?> Get(string name, CancellationToken cancellationToken = default)
    {
        return await this.context.Families.SingleOrDefaultAsync(i => i.Name == name, cancellationToken);
    }

    public async Task Save(FamilyEntity family, CancellationToken cancellationToken = default)
    {
        await this.context.Save(family, cancellationToken);
    }
}
