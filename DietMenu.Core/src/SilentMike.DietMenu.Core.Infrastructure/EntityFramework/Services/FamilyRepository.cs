﻿namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class FamilyRepository : IFamilyRepository
{
    private readonly DietMenuDbContext context;

    public FamilyRepository(DietMenuDbContext context) => (this.context) = (context);

    public FamilyEntity? Get(Guid id)
        => this.context.Families.SingleOrDefault(family => family.Id == id);

    public void Save(FamilyEntity family)
    {
        this.context.Upsert(family);

        this.context.SaveChanges();
    }
}
