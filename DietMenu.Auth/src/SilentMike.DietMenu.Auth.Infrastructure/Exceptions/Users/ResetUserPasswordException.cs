namespace SilentMike.DietMenu.Auth.Infrastructure.Exceptions.Users;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Infrastructure.Common;

[Serializable]
public sealed class ResetUserPasswordException : InfrastructureException
{
    public override string Code => ErrorCodes.RESET_USER_PASSWORD_ERROR;

    public ResetUserPasswordException(Guid id, string message)
        : base($"Unable to reset password user with id '{id}': {message}")
        => this.Id = id;

    private ResetUserPasswordException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
