namespace SilentMike.DietMenu.Core.Application.Exceptions;

using System;
using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

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
