namespace SilentMike.DietMenu.Core.Application.Exceptions.Auth;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class UnconfirmedEmailException : ApplicationException
{
    public override string Code => ErrorCodes.UNCONFIRMED_EMAIL;

    public UnconfirmedEmailException(string email)
        : base($"User email {email} is not confirmed")
    {
    }

    private UnconfirmedEmailException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
