namespace SilentMike.DietMenu.Core.Infrastructure.Dapper.Interfaces;

using System.Text;

internal interface IQuery
{
    StringBuilder QueryBuilder { get; }
}
