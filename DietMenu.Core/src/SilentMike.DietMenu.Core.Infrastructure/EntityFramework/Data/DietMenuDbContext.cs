namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Entities;

[ExcludeFromCodeCoverage]
internal sealed class DietMenuDbContext : DbContext, IDietMenuDbContext
{
    public DbSet<FamilyEntity> Families => this.Set<FamilyEntity>();
    public DbSet<IngredientEntity> Ingredients => this.Set<IngredientEntity>();

    public DietMenuDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.HasDefaultSchema("SilentMike");

        base.OnModelCreating(modelBuilder);
    }
}
