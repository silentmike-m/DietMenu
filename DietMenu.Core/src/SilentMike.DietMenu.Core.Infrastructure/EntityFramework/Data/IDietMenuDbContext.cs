﻿namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Entities;

internal interface IDietMenuDbContext
{
    DbSet<FamilyEntity> Families { get; }
    DbSet<IngredientEntity> Ingredients { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
