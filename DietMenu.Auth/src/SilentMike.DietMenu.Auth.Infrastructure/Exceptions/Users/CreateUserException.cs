namespace SilentMike.DietMenu.Auth.Infrastructure.Exceptions.Users;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Infrastructure.Common;

[Serializable]
public sealed class CreateUserException : InfrastructureException
{
    public override string Code => ErrorCodes.CREATE_USER_ERROR;

    public CreateUserException(string email, string message)
        : base($"Unable to create user '{email}': {message}")
    {
    }

    private CreateUserException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
