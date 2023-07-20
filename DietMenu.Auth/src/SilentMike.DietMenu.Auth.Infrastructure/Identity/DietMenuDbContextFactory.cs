namespace SilentMike.DietMenu.Auth.Infrastructure.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;

internal sealed class DietMenuDbContextFactory : IDesignTimeDbContextFactory<DietMenuDbContext>
{
    public DietMenuDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DietMenuDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=dietmenu-identity;User Id=sa;Password=P@ssw0rd;MultipleActiveResultSets=true");

        return new DietMenuDbContext(optionsBuilder.Options);
    }
}
