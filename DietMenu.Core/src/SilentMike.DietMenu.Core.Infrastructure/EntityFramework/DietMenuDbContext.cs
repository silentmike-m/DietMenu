namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models;

[ExcludeFromCodeCoverage]
internal sealed class DietMenuDbContext : DbContext
{
    public DbSet<CoreEntity> Core => Set<CoreEntity>();
    public DbSet<CoreIngredientEntity> CoreIngredients => Set<CoreIngredientEntity>();
    public DbSet<CoreIngredientTypeEntity> CoreIngredientTypes => Set<CoreIngredientTypeEntity>();
    public DbSet<CoreMealTypeEntity> CoreMealTypes => Set<CoreMealTypeEntity>();
    public DbSet<FamilyEntity> Families => Set<FamilyEntity>();
    public DbSet<IngredientEntity> Ingredients => Set<IngredientEntity>();
    public DbSet<IngredientRow> IngredientRows => Set<IngredientRow>();
    public DbSet<IngredientTypeEntity> IngredientTypes => Set<IngredientTypeEntity>();
    public DbSet<IngredientTypeRow> IngredientTypeRows => Set<IngredientTypeRow>();
    public DbSet<MealTypeEntity> MealTypes => Set<MealTypeEntity>();
    public DbSet<MealTypeRow> MealTypeRows => Set<MealTypeRow>();
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

    public void Upsert<TEntity>(TEntity entity)
        where TEntity : class
    {
        var list = new List<TEntity>
        {
            entity,
        };

        this.Upsert<TEntity>(list);
    }

    public void Upsert<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
    {
        foreach (var entity in entities)
        {
            var track = this.Entry(entity);

            switch (track.State)
            {
                case EntityState.Added or EntityState.Detached:
                    this.Add(entity);
                    break;
                case EntityState.Modified:
                    this.Update(entity);
                    break;
            }
        }
    }
}
