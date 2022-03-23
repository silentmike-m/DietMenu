namespace SilentMike.DietMenu.Core.Infrastructure.Dapper.Interfaces;

using System.Text;

internal interface IFilteredQuery
{
    StringBuilder QueryBuilder { get; }
}
