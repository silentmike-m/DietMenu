namespace SilentMike.DietMenu.Core.Infrastructure.Dapper.Interfaces;

using System.Text;

internal interface IOrderedQuery
{
    StringBuilder QueryBuilder { get; }
}
