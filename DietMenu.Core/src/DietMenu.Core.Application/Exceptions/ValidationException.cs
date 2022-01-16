namespace DietMenu.Core.Application.Exceptions;

using System.Runtime.Serialization;
using DietMenu.Core.Application.Common.Constants;
using FluentValidation.Results;

[Serializable]
public sealed class ValidationException : Core.Application.Common.ApplicationException
{
    public override string Code => ErrorCodes.VALIDATION_FAILED;
    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this("Errors have occurred during validation process")
    {
        var errors = failures
            .GroupBy(e => e.ErrorCode, e => e.ErrorMessage)
            .ToDictionary(
                failureGroup => failureGroup.Key,
                failureGroup => failureGroup.ToArray()
            );

        Errors = errors;
    }

    public ValidationException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }

    private ValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
