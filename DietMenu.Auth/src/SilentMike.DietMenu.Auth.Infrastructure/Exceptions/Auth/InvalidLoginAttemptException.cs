namespace SilentMike.DietMenu.Auth.Infrastructure.Exceptions.Auth;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Infrastructure.Common;

[Serializable]
public sealed class InvalidLoginAttemptException : InfrastructureException
{
    public override string Code => ErrorCodes.INVALID_LOGIN_ATTEMPT;

    public InvalidLoginAttemptException()
        : base("Invalid login attempt")
    {
    }

    private InvalidLoginAttemptException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
