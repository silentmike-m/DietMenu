namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Middlewares;

using System.Diagnostics.CodeAnalysis;
using global::MassTransit;
using GreenPipes;
using Microsoft.Extensions.Logging;

[ExcludeFromCodeCoverage]
internal sealed class RetryFilter<T> : IFilter<ConsumeContext<T>>
    where T : class
{
    private const int RETRY_DELAY_IN_MINUTES = 5;

    private readonly ILogger<RetryFilter<T>> logger;

    public RetryFilter(ILogger<RetryFilter<T>> logger)
        => (this.logger) = (logger);

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        try
        {
            await next.Send(context);
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            if (context.ExpirationTime.HasValue && DateTime.UtcNow <= context.ExpirationTime.Value.ToUniversalTime())
            {
                await context.Defer(TimeSpan.FromMinutes(RETRY_DELAY_IN_MINUTES));
            }
            else
            {
                throw;
            }
        }
    }

    public void Probe(ProbeContext context)
    {
        // Method intentionally left empty.
    }
}
