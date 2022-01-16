namespace DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

using DietMenu.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

internal interface IApplicationDbContext
{
    DbSet<FamilyEntity> Families { get; }

    Task Save<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
}
