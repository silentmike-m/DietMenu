namespace SilentMike.DietMenu.Core.Infrastructure.Dapper.Interfaces;

using System.Text;

internal interface IPagedQuery
{
    StringBuilder QueryBuilder { get; }
}
