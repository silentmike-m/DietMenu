namespace SilentMike.DietMenu.Core.UnitTests.Services.DbSetMoq;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;

internal static class DbSetMoqExtensions
{
    public static IReturnsResult<T> ReturnsDbSet<T, TEntity>(this ISetup<T, DbSet<TEntity>> setupResult, IEnumerable<TEntity> entities, Mock<DbSet<TEntity>>? dbSetMock = null)
        where T : class where TEntity : class
    {
        dbSetMock ??= new Mock<DbSet<TEntity>>();

        ConfigureMock(dbSetMock, entities);

        return setupResult.Returns(dbSetMock.Object);
    }

    private static void ConfigureMock<TEntity>(Mock dbSetMock, IEnumerable<TEntity> entities)
        where TEntity : class
    {
        var entitiesAsQueryable = entities.AsQueryable();

        dbSetMock.As<IAsyncEnumerable<TEntity>>()
            .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
            .Returns(new InMemoryDbAsyncEnumerator<TEntity>(entitiesAsQueryable.GetEnumerator()));

        dbSetMock.As<IQueryable<TEntity>>()
            .Setup(m => m.Provider)
            .Returns(new InMemoryAsyncQueryProvider(entitiesAsQueryable.Provider));

        dbSetMock.As<IQueryable<TEntity>>()
            .Setup(m => m.Expression)
            .Returns(entitiesAsQueryable.Expression);

        dbSetMock.As<IQueryable<TEntity>>()
            .Setup(m => m.ElementType)
            .Returns(entitiesAsQueryable.ElementType);

        dbSetMock.As<IQueryable<TEntity>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => entitiesAsQueryable.GetEnumerator());
    }
}
