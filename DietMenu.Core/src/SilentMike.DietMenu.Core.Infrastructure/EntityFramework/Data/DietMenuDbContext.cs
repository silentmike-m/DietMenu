namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models;

[ExcludeFromCodeCoverage]
internal sealed class DietMenuDbContext : DbContext, IDietMenuDbContext
{
    public DbSet<Family> Families => this.Set<Family>();

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
