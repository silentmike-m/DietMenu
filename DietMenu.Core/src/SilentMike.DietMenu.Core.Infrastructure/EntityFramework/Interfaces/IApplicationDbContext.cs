namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;

internal interface IApplicationDbContext
{
    DbSet<FamilyEntity> Families { get; }

    Task Save<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
}
