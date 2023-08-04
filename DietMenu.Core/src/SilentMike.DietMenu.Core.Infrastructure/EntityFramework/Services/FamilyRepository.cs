namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models;
using SilentMike.DietMenu.Core.Infrastructure.Exceptions.Families;

internal sealed class FamilyRepository : IFamilyRepository
{
    private readonly IDietMenuDbContext context;

    public FamilyRepository(IDietMenuDbContext context)
        => this.context = context;

    public async Task AddFamilyAsync(FamilyEntity entity, CancellationToken cancellationToken = default)
    {
        var family = await this.context.Families.SingleOrDefaultAsync(family => family.InternalId == entity.Id, cancellationToken);

        if (family is not null)
        {
            throw new FamilyAlreadyExistsException(entity.Id);
        }

        family = new Family
        {
            InternalId = entity.Id,
        };

        await this.context.Families.AddAsync(family, cancellationToken);

        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        => await this.context.Families.AnyAsync(family => family.InternalId == id, cancellationToken);
}
