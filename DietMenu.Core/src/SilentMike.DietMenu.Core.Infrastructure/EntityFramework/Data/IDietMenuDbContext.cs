namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models;

internal interface IDietMenuDbContext
{
    DbSet<Family> Families { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
