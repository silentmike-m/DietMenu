namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

internal sealed class DietMenuDbContextFactory : IDesignTimeDbContextFactory<DietMenuDbContext>
{
    public DietMenuDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DietMenuDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=silentmike-dietmenu;User Id=sa;Password=P@ssw0rd;MultipleActiveResultSets=true");

        return new DietMenuDbContext(optionsBuilder.Options);
    }
}
