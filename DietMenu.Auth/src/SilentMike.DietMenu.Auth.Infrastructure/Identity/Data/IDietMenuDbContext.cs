namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal interface IDietMenuDbContext
{
    DbSet<DietMenuFamily> Families { get; }
}
