namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Services;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

internal sealed class FamilyRepository : IFamilyRepository
{
    private readonly IDietMenuDbContext context;

    public FamilyRepository(IDietMenuDbContext context)
        => this.context = context;

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        => await this.context.Families
            .AsNoTracking()
            .AnyAsync(family => family.FamilyId == id, cancellationToken);
}
