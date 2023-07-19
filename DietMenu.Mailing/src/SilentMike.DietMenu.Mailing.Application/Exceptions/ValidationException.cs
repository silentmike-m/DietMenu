namespace SilentMike.DietMenu.Mailing.Application.Exceptions;

using System.Runtime.Serialization;
using FluentValidation.Results;
using SilentMike.DietMenu.Mailing.Application.Common;

[Serializable]
public sealed class ValidationException : ApplicationException
{
    public override string Code => ErrorCodes.VALIDATION_FAILED;
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

        this.Errors = errors;
    }

    private ValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
