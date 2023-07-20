namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

[ExcludeFromCodeCoverage]
internal sealed class DietMenuDbContext : IdentityDbContext<User>, IDietMenuDbContext
{
    public DbSet<Family> Families => this.Set<Family>();

    public DietMenuDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.HasDefaultSchema("SilentMike");

        base.OnModelCreating(builder);
    }
}
