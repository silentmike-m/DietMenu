namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Services;

using global::AutoMapper;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Auth.Domain.Entities;
using SilentMike.DietMenu.Auth.Domain.Services;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class FamilyRepository : IFamilyRepository
{
    private readonly IDietMenuDbContext context;
    private readonly IMapper Mapper;

    public FamilyRepository(IDietMenuDbContext context, IMapper mapper)
    {
        this.context = context;
        this.Mapper = mapper;
    }

    public async Task<FamilyEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var family = await this.context.Families.SingleOrDefaultAsync(family => family.Id == id, cancellationToken);

        if (family is null)
        {
            return null;
        }

        var result = this.Mapper.Map<FamilyEntity>(family);

        return result;
    }

    public async Task SaveAsync(FamilyEntity family, CancellationToken cancellationToken = default)
    {
        var entity = await this.context.Families.SingleOrDefaultAsync(entity => entity.Id == family.Id, cancellationToken);

        if (entity is null)
        {
            entity = new Family
            {
                Id = family.Id,
                Name = family.Name,
            };

            await this.context.Families.AddAsync(entity, cancellationToken);
        }
        else
        {
            entity.Name = family.Name;
            this.context.Families.Update(entity);
        }

        await this.context.SaveChangesAsync(cancellationToken);
    }
}
