namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Middlewares;

using System.Diagnostics.CodeAnalysis;
using global::MassTransit;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Application.Extensions;

[ExcludeFromCodeCoverage]
internal sealed class ExceptionLoggerFilter<T> : IFilter<ConsumeContext<T>>
    where T : class
{
    private readonly ILogger<ExceptionLoggerFilter<T>> logger;

    public ExceptionLoggerFilter(ILogger<ExceptionLoggerFilter<T>> logger)
        => this.logger = logger;

    public void Probe(ProbeContext context)
    {
        // Method intentionally left empty.
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("ConversationId", context.ConversationId ?? Guid.Empty),
            ("CorrelationId", context.CorrelationId ?? Guid.Empty),
            ("MessageId", context.MessageId ?? Guid.Empty),
            ("RequestId", context.RequestId ?? Guid.Empty)
        );

        await next.Send(context);
    }
}
