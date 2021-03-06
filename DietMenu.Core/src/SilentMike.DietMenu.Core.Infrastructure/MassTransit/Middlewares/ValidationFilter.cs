namespace SilentMike.DietMenu.Core.Infrastructure.MassTransit.Middlewares;

using FluentValidation;
using global::MassTransit;
using GreenPipes;
using ValidationException = SilentMike.DietMenu.Core.Application.Exceptions.ValidationException;

internal sealed class ValidationFilter<T> : IFilter<ConsumeContext<T>>
    where T : class
{
    private readonly IEnumerable<IValidator<T>> validators;

    public ValidationFilter(IEnumerable<IValidator<T>> validators) =>
        (this.validators) = (validators);

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

    public void Probe(ProbeContext context)
    {
        // Method intentionally left empty.
    }
}
