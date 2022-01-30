namespace SilentMike.DietMenu.Auth.Application.Exceptions.Users;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Auth.Application.Common.ApplicationException;

[Serializable]
public sealed class LoginUserException : ApplicationException
{
    public override string Code => ErrorCodes.LOGIN_USER_ERROR;

    public LoginUserException()
        : base("Invalid login attempt")
    {
    }

    private LoginUserException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
