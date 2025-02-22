﻿namespace SilentMike.DietMenu.Mailing.Application.Exceptions;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Mailing.Application.Common;

[Serializable]
public sealed class UnhandledErrorException : ApplicationException
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
