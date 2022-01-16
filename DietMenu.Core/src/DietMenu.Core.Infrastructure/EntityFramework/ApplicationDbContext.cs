namespace DietMenu.Core.Infrastructure.EntityFramework;

using System.Reflection;
using DietMenu.Core.Domain.Entities;
using DietMenu.Core.Infrastructure.EntityFramework.Interfaces;
using DietMenu.Core.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

internal sealed class ApplicationDbContext : IdentityDbContext<DietMenuUser, DietMenuRole, Guid>, IApplicationDbContext
{
    public DbSet<FamilyEntity> Families => Set<FamilyEntity>();

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.HasDefaultSchema("SilentMike");

        base.OnModelCreating(builder);

    }

    public async Task Save<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
    {
        this.Update(entity);
        await this.SaveChangesAsync(cancellationToken);
    }

}
