namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Extensions;

using System.Linq.Expressions;

internal static class DbSetExtensions
{
    public static int GetItemsCount<TEntity>(this IQueryable<TEntity> set, Expression<Func<TEntity, bool>> filter)
    {
        return set.Count(filter);
    }

    public static IQueryable<TEntity> GetFiltered<TEntity>(this IQueryable<TEntity> set, Expression<Func<TEntity, bool>> filter)
    {
        return set.Where(filter);
    }

    public static IQueryable<TEntity> GetOrdered<TEntity>(this IQueryable<TEntity> set, Expression<Func<TEntity, object>> orderBy, bool isdDescending)
    {
        if (isdDescending)
        {
            return set.OrderByDescending(orderBy);
        }

        return set.OrderBy(orderBy);
    }

    public static IQueryable<TEntity> GetPaged<TEntity>(this IQueryable<TEntity> set, int currentPageNumber, bool isPaged, int pageSize)
    {
        return isPaged ? set.Skip(currentPageNumber * pageSize).Take(pageSize) : set;
    }
}
