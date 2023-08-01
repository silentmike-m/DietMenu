namespace SilentMike.DietMenu.Auth.Infrastructure.Identity;

using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

internal sealed class PersistedGrantDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
{
    private const string CONNECTION_STRING = "Server=localhost,1433;Database=dietmenu-identity;User Id=sa;Password=P@ssw0rd;MultipleActiveResultSets=true";

    public PersistedGrantDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PersistedGrantDbContext>();

        var migrationsAssembly = Assembly.GetExecutingAssembly().GetName().Name;

        optionsBuilder.UseSqlServer(CONNECTION_STRING, builder => builder.MigrationsAssembly(migrationsAssembly));

        return new PersistedGrantDbContext(optionsBuilder.Options, new OperationalStoreOptions());
    }
}
