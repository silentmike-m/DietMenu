namespace SilentMike.DietMenu.Core.UnitTests.Services;

using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

internal sealed class DietMenuDbContextFactory : IDisposable
{
    public DietMenuDbContext Context { get; }

    private readonly DbConnection connection;

    public DietMenuDbContextFactory(params object[] entities)
    {
        connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var contextOptions = new DbContextOptionsBuilder<DietMenuDbContext>()
            .UseSqlite(connection)
            .Options;

        using var context = new DietMenuDbContext(contextOptions);

        context.Database.EnsureCreated();
        context.AddRange(entities);
        _ = context.SaveChanges();

        this.Context = new DietMenuDbContext(contextOptions);
    }

    public void Dispose()
    {
        this.connection.Dispose();
    }
}
