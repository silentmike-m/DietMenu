namespace DietMenu.Api.Infrastructure.EntityFramework;

using System.Reflection;
using DietMenu.Api.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

internal sealed class ApplicationDbContext : IdentityDbContext<DietMenuUser, DietMenuRole, Guid>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.HasDefaultSchema("SilentMike");

        base.OnModelCreating(builder);
    }
}
