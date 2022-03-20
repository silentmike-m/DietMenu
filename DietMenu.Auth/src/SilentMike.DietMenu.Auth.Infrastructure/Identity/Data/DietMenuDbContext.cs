namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

[ExcludeFromCodeCoverage]
internal sealed class DietMenuDbContext : IdentityDbContext<DietMenuUser>
{
    public DbSet<DietMenuFamily> Families => Set<DietMenuFamily>();

    public DietMenuDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("SilentMike");

        base.OnModelCreating(builder);

    }
}
