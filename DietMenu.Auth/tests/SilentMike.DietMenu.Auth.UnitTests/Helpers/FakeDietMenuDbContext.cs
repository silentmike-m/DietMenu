namespace SilentMike.DietMenu.Auth.UnitTests.Helpers;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;

public class FakeDietMenuDbContext : IDisposable
{
    internal DietMenuDbContext? Context;

    protected FakeDietMenuDbContext(params object[] entities)
    {
        var dbName = Guid.NewGuid().ToString();

        var contextOptions = new DbContextOptionsBuilder<DietMenuDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        this.Context = new DietMenuDbContext(contextOptions);

        this.Context.Database.EnsureCreated();

        if (entities.Any())
        {
            this.Context.AddRange(entities);
            _ = this.Context.SaveChanges();
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.Context?.Dispose();
            this.Context = null;
        }
    }
}
