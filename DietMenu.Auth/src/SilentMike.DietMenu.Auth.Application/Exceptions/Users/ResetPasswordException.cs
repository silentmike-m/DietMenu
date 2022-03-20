namespace SilentMike.DietMenu.Auth.Application.Exceptions.Users;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Auth.Application.Common.ApplicationException;

[Serializable]
public sealed class ResetPasswordException : ApplicationException
{
    public override string Code => ErrorCodes.RESET_PASSWORD;

    public ResetPasswordException(string email, string message)
        : base($"Unable to reset password user with email '{email}': {message}")
    {
    }

    private ResetPasswordException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
