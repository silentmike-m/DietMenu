namespace SilentMike.DietMenu.Core.Application.Exceptions.Auth;

using System;
using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class ConfirmEmailException : ApplicationException
{
    public override string Code => ErrorCodes.ACTIVATE_EMAIL;


    public ConfirmEmailException(string message)
        : base($"Unable to activate email: {message}")
    {
        this.Id = Guid.Empty;
    }

    private ConfirmEmailException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
