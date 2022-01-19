namespace SilentMike.DietMenu.Core.UnitTests.Services.DbSetMoq;

using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

internal sealed class InMemoryAsyncQueryProvider : IAsyncQueryProvider
{
    private readonly IQueryProvider innerQueryProvider;

    public InMemoryAsyncQueryProvider(IQueryProvider innerQueryProvider)
    {
        this.innerQueryProvider = innerQueryProvider;
    }

    public IQueryable CreateQuery(Expression expression)
        => this.innerQueryProvider.CreateQuery(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        => this.innerQueryProvider.CreateQuery<TElement>(expression);

    public object? Execute(Expression expression)
        => this.innerQueryProvider.Execute(expression);

    public TResult Execute<TResult>(Expression expression)
        => this.innerQueryProvider.Execute<TResult>(expression);

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        var result = Execute(expression);

        var expectedResultType = typeof(TResult).GetGenericArguments()?.FirstOrDefault();
        if (expectedResultType == null)
        {
            return default!;
        }

        return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
            .MakeGenericMethod(expectedResultType)
            .Invoke(null, new[] { result })!;
    }
}
