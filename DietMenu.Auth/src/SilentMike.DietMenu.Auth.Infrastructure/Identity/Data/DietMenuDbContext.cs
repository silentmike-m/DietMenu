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

    public DietMenuDbContext(DbContextOptions<DietMenuDbContext> options)
        : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>()
            .HaveConversion<string>();

        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.HasDefaultSchema("SilentMike");

        base.OnModelCreating(builder);
    }
}
