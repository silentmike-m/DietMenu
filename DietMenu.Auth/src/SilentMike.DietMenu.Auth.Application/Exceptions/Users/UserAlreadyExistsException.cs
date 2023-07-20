namespace SilentMike.DietMenu.Auth.Application.Exceptions.Users;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Application.Common;

[Serializable]
public sealed class UserAlreadyExistsException : ApplicationException
{
    public override string Code => ErrorCodes.USER_ALREADY_EXISTS;

    public UserAlreadyExistsException(Guid id)
        : base($"User with id '{id}' already exists")
        => this.Id = id;

    public UserAlreadyExistsException(string email)
        : base($"User with email '{email}' already exists")
    {
    }

    private UserAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
