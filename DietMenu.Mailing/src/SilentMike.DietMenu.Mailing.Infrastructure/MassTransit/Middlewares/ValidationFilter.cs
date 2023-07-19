namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Middlewares;

using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using global::MassTransit;
using ValidationException = SilentMike.DietMenu.Mailing.Application.Exceptions.ValidationException;

[ExcludeFromCodeCoverage]
internal sealed class ValidationFilter<T> : IFilter<ConsumeContext<T>>
    where T : class
{
    private readonly IEnumerable<IValidator<T>> validators;

    public ValidationFilter(IEnumerable<IValidator<T>> validators) =>
        this.validators = validators;

    public void Probe(ProbeContext context)
    {
        // Method intentionally left empty.
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        if (this.validators.Any())
        {
            var validationContext = new ValidationContext<T>(context.Message);

            var validation = this.validators.Select(v => v.ValidateAsync(validationContext));
            var validationResults = await Task.WhenAll(validation);

            var failures = validationResults
                .SelectMany(vr => vr.Errors)
                .Where(vr => vr is not null)
                .ToList();

            if (failures.Count > 0)
            {
                throw new ValidationException(failures);
            }
        }

        await next.Send(context);
    }
}
