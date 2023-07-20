namespace SilentMike.DietMenu.Auth.UnitTests.Helpers;

using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;

public class FakeDietMenuDbContext : IDisposable
{
    private DbConnection? connection;
    internal DietMenuDbContext? Context { get; private set; }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Create(params object[] entities)
    {
        this.connection = new SqliteConnection("Filename=:memory:");
        this.connection.Open();

        var contextOptions = new DbContextOptionsBuilder<DietMenuDbContext>()
            .UseSqlite(this.connection)
            .Options;

        using var context = new DietMenuDbContext(contextOptions);

        context.Database.EnsureCreated();
        context.AddRange(entities);
        _ = context.SaveChanges();

        this.Context = new DietMenuDbContext(contextOptions);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.connection?.Dispose();
            this.connection = null;

            this.Context?.Dispose();
            this.Context = null;
        }
    }
}
