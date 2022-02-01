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
        var track = this.Entry(entity);

        switch (track.State)
        {
            case EntityState.Added or EntityState.Detached:
                this.Add(entity);
                await this.SaveChangesAsync(cancellationToken);
                break;
            case EntityState.Modified:
                this.Update(entity);
                await this.SaveChangesAsync(cancellationToken);
                break;
        }
    }

    public async Task Save<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var saveChanges = false;

        foreach (var entity in entities)
        {
            var track = this.Entry(entity);

            switch (track.State)
            {
                case EntityState.Added or EntityState.Detached:
                    this.Add(entity);
                    saveChanges = true;
                    break;
                case EntityState.Modified:
                    this.Update(entity);
                    saveChanges = true;
                    break;
            }
        }

        if (saveChanges)
        {
            await this.SaveChangesAsync(cancellationToken);
        }
    }
}
