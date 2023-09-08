namespace SilentMike.DietMenu.Core.Application.Exceptions;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common;

[Serializable]
public sealed class UnhandledErrorException : ApplicationException
{
    public override string Code => ErrorCodes.UNKNOWN_ERROR;

    public UnhandledErrorException()
        : base("Unhandled error has occurred")
    {
    }

    private UnhandledErrorException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
