namespace SilentMike.DietMenu.Auth.UnitTests.Services;

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;

internal static class DbSetMoqExtensions
{
    public static Mock<DbSet<T>> ToDbSetMoq<T>(this IEnumerable<T> self)
        where T : class
    {
        var queryable = self.AsQueryable();

        var dbSetMock = new Mock<DbSet<T>>();

        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider)
            .Returns(queryable.Provider);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression)
            .Returns(queryable.Expression);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType)
            .Returns(queryable.ElementType);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator())
            .Returns(queryable.GetEnumerator());

        return dbSetMock;
    }
}
