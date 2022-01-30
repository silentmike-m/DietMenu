namespace SilentMike.DietMenu.Auth.Application.Exceptions.Users;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Auth.Application.Common.ApplicationException;

[Serializable]
public sealed class UserNotFoundException : ApplicationException
{
    public override string Code => ErrorCodes.USER_NOT_FOUND;

    public UserNotFoundException(string email)
        : base($"User with email {email} not found")
    {
    }

    public UserNotFoundException(Guid id)
        : base($"User with id {id} not found")
    {
    }

    private UserNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
