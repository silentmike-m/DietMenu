namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;

[ExcludeFromCodeCoverage]
internal sealed class DietMenuDbContext : DbContext, IDietMenuDbContext
{
    public DbSet<FamilyEntity> Families => Set<FamilyEntity>();
    public DbSet<MealTypeEntity> MealTypes => Set<MealTypeEntity>();

    public DietMenuDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.HasDefaultSchema("SilentMike");

        base.OnModelCreating(modelBuilder);

    }

    public async Task Save<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        this.Update(entity);
        await this.SaveChangesAsync(cancellationToken);
    }

    public async Task Save<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        this.UpdateRange(entities);
        await this.SaveChangesAsync(cancellationToken);
    }
}
