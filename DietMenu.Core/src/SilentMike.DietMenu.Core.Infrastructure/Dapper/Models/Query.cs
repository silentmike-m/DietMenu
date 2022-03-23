namespace SilentMike.DietMenu.Core.Infrastructure.Dapper.Models;

using System.Text;
using SilentMike.DietMenu.Core.Infrastructure.Dapper.Interfaces;

internal sealed record Query : IQuery, IFilteredQuery, IOrderedQuery, IPagedQuery
{
    public StringBuilder QueryBuilder { get; }

    public Query(string query)
    {
        this.QueryBuilder = new StringBuilder(query);
    }
}
