namespace DietMenu.Core.UnitTests.Services.DbSetMoq;

using System.Collections.Generic;
using System.Threading.Tasks;

internal sealed class InMemoryDbAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> innerEnumerator;
    private bool disposed = false;

    public T Current => this.innerEnumerator.Current;

    public InMemoryDbAsyncEnumerator(IEnumerator<T> enumerator)
    {
        this.innerEnumerator = enumerator;
    }

    public ValueTask DisposeAsync()
    {
        Dispose(true);
        return new ValueTask();
    }

    public ValueTask<bool> MoveNextAsync()
        => new(Task.FromResult(this.innerEnumerator.MoveNext()));

    private void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                // Dispose managed resources.
                this.innerEnumerator.Dispose();
            }

            this.disposed = true;
        }
    }
}
