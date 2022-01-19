namespace SilentMike.DietMenu.Mailing.Application.Exceptions;

using System.Runtime.Serialization;
using FluentValidation.Results;
using ApplicationException = SilentMike.DietMenu.Mailing.Application.Common.ApplicationException;

[Serializable]
public sealed class ValidationException : ApplicationException
{
    public override string Code => "validation_failed";
    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("Errors have occurred during validation process")
    {
        var errors = failures
            .GroupBy(e => e.ErrorCode, e => e.ErrorMessage)
            .ToDictionary(
                failureGroup => failureGroup.Key,
                failureGroup => failureGroup.ToArray()
            );

        Errors = errors;
    }

    private ValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
