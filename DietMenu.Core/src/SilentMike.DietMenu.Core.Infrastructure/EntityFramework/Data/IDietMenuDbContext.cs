namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;

internal interface IDietMenuDbContext
{
    DbSet<FamilyEntity> Families { get; }
    DbSet<IngredientEntity> Ingredients { get; }
    DbSet<IngredientTypeEntity> IngredientTypes { get; }
    DbSet<MealTypeEntity> MealTypes { get; }

    Task Save<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class;

    Task Save<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class;
}
