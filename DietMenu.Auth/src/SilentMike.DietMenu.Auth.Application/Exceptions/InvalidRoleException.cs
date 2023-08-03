namespace SilentMike.DietMenu.Auth.Application.Exceptions;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Application.Common;

[Serializable]
public sealed class InvalidRoleException : ApplicationException
{
    public override string Code => ErrorCodes.INVALID_ROLE;

    public InvalidRoleException(string role)
        : base($"User role '{role}' is not enough to process request")
    {
    }

    private InvalidRoleException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
