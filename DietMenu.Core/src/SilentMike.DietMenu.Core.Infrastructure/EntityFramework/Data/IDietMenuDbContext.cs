namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;

internal interface IDietMenuDbContext
{
    DbSet<FamilyEntity> Families { get; }

    Task Save<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
}
