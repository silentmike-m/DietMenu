namespace DietMenu.Core.Application.Exceptions;

using System.Runtime.Serialization;
using DietMenu.Core.Application.Common.Constants;

[Serializable]
public sealed class UnhandledErrorException : Core.Application.Common.ApplicationException
{
    public override string Code => ErrorCodes.UNHANDLED_ERROR;

    public UnhandledErrorException()
        : base("Unhandled error has occurred")
    {
    }

    private UnhandledErrorException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
