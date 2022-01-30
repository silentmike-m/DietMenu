namespace SilentMike.DietMenu.Auth.Application.Exceptions.Users;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Auth.Application.Common.ApplicationException;

[Serializable]
public sealed class CreateUserException : ApplicationException
{
    public override string Code => ErrorCodes.CREATE_USER_ERROR;

    public CreateUserException(string email, string message)
        : base($"Unable to create user {email}: {message}")
    {
    }

    private CreateUserException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
