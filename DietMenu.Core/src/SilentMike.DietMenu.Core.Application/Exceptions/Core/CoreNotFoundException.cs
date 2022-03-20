namespace SilentMike.DietMenu.Core.Application.Exceptions.Core;

using System;
using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class CoreNotFoundException : ApplicationException
{
    public override string Code => ErrorCodes.CORE_NOT_FOUND;

    public CoreNotFoundException()
        : base("System core has not been found")
    {
    }

    private CoreNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
