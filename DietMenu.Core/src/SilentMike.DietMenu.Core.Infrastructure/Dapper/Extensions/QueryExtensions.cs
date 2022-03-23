namespace SilentMike.DietMenu.Core.Infrastructure.Dapper.Extensions;

using System.Text;
using SilentMike.DietMenu.Core.Infrastructure.Dapper.Interfaces;
using SilentMike.DietMenu.Core.Infrastructure.Dapper.Models;
using SilentMike.DietMenu.Core.Infrastructure.Dapper.Services;

internal static class QueryExtensions
{
    public static IFilteredQuery GetFiltered(this IQuery query, string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return (IFilteredQuery)query;
        }

        query.QueryBuilder.Append($" WHERE {filter}");

        return (IFilteredQuery)query;
    }

    public static IFilteredQuery GetFiltered(this IQuery query, ICollection<Filter> filters)
    {
        if (filters.Any())
        {
            query.QueryBuilder.Append(" WHERE 1 = 1");

            foreach (var filter in filters)
            {
                query.QueryBuilder.AddFilter(filter);
            }
        }

        return (IFilteredQuery)query;
    }

    private static void AddFilter(this StringBuilder builder, Filter filter)
    {
        builder.Append($" {filter.FilterJoinType} {filter.PropertyName}");

        switch (filter.FilterType)
        {
            case FilterType.Contains:
                builder.Append(" LIKE");

                builder.Append(filter.IsString
                    ? $" N'%{filter.Value}%'"
                    : $" {filter.Value}");
                break;
            case FilterType.Equals:
                builder.Append(" =");

                builder.Append(filter.IsString
                    ? $" N'{filter.Value}'"
                    : $" {filter.Value}");
                break;
        }
    }

    public static IOrderedQuery GetOrdered(this IFilteredQuery query, string orderBy, bool isdDescending)
    {
        query.QueryBuilder.GetOrdered(orderBy, isdDescending);

        return (IOrderedQuery)query;
    }

    public static IOrderedQuery GetOrdered(this IQuery query, string orderBy, bool isdDescending)
    {
        query.QueryBuilder.GetOrdered(orderBy, isdDescending);

        return (IOrderedQuery)query;
    }

    public static IPagedQuery GetPaged(this IFilteredQuery query, int currentPageNumber, bool isPaged, int pageSize)
    {
        query.QueryBuilder.GetPaged(currentPageNumber, isPaged, pageSize);

        return (IPagedQuery)query;
    }

    public static IPagedQuery GetPaged(this IOrderedQuery query, int currentPageNumber, bool isPaged, int pageSize)
    {
        query.QueryBuilder.GetPaged(currentPageNumber, isPaged, pageSize);

        return (IPagedQuery)query;
    }

    public static IPagedQuery GetPaged(this IQuery query, int currentPageNumber, bool isPaged, int pageSize)
    {
        query.QueryBuilder.GetPaged(currentPageNumber, isPaged, pageSize);

        return (IPagedQuery)query;
    }

    private static void GetOrdered(this StringBuilder builder, string orderBy, bool isdDescending)
    {
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            builder.Append(isdDescending
                ? $" ORDER BY [{orderBy}] DESC"
                : $" ORDER BY [{orderBy}]");
        }
    }

    private static void GetPaged(this StringBuilder builder, int currentPageNumber, bool isPaged, int pageSize)
    {
        if (isPaged)
        {
            currentPageNumber = currentPageNumber < 0 ? 0 : currentPageNumber;
            pageSize = pageSize <= 0 ? 1 : pageSize;

            builder.Append($" OFFSET {currentPageNumber * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");
        }
    }
}
