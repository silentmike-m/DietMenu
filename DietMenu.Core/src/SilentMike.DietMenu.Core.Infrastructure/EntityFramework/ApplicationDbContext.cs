namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;
using SilentMike.DietMenu.Core.Infrastructure.Identity.Models;

[ExcludeFromCodeCoverage]
internal sealed class ApplicationDbContext : IdentityDbContext<DietMenuUser, DietMenuRole, Guid>, IApplicationDbContext
{
    public DbSet<FamilyEntity> Families => Set<FamilyEntity>();

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.HasDefaultSchema("SilentMike");

        base.OnModelCreating(builder);

    }

    public async Task Save<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
    {
        this.Update(entity);
        await this.SaveChangesAsync(cancellationToken);
    }

}
