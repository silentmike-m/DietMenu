namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;

[ExcludeFromCodeCoverage]
internal sealed class DietMenuDbContext : DbContext
{
    public DbSet<FamilyEntity> Families => Set<FamilyEntity>();
    public DbSet<IngredientEntity> Ingredients => Set<IngredientEntity>();
    public DbSet<IngredientTypeEntity> IngredientTypes => Set<IngredientTypeEntity>();
    public DbSet<MealTypeEntity> MealTypes => Set<MealTypeEntity>();
    public DbSet<RecipeEntity> Recipes => Set<RecipeEntity>();
    public DbSet<RecipeIngredientEntity> RecipeIngredients => Set<RecipeIngredientEntity>();

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
        var entities = new List<TEntity>
        {
            entity,
        }.AsEnumerable();

        await this.Save(entities, cancellationToken);
    }

    public async Task Save<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var saveChanges = this.UpsertEntities(entities);

        if (saveChanges)
        {
            await this.SaveChangesAsync(cancellationToken);
        }
    }

    private bool UpsertEntities<TEntity>(IEnumerable<TEntity> entities)
    {
        var saveChanges = false;

        foreach (var entity in entities)
        {
            var track = this.Entry(entity!);

            switch (track.State)
            {
                case EntityState.Added or EntityState.Detached:
                    this.Add(entity!);
                    saveChanges = true;
                    break;
                case EntityState.Modified:
                    this.Update(entity!);
                    saveChanges = true;
                    break;
            }
        }

        return saveChanges;
    }
}
