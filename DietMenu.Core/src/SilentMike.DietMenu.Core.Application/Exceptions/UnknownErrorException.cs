namespace SilentMike.DietMenu.Core.Application.Exceptions;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common;

[Serializable]
public sealed class UnknownErrorException : ApplicationException
{
    public override string Code => ErrorCodes.UNHANDLED_ERROR;

    public UnknownErrorException()
        : base("Unknown error has occurred")
    {
    }

    public UnknownErrorException(string message)
        : base(message)
    {
    }

    private UnknownErrorException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
