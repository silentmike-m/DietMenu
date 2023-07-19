namespace SilentMike.DietMenu.Mailing.Application.Common.Behaviours;

using FluentValidation;
using ValidationException = SilentMike.DietMenu.Mailing.Application.Exceptions.ValidationException;

internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) =>
        this.validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (this.validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validation = this.validators.Select(v => v.ValidateAsync(context, cancellationToken));
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

        return await next();
    }
}
