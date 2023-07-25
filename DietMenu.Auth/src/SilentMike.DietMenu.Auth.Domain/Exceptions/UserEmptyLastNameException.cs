namespace SilentMike.DietMenu.Auth.Domain.Exceptions;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Domain.Common;
using SilentMike.DietMenu.Auth.Domain.Common.Constants;

[Serializable]
public sealed class UserEmptyLastNameException : DomainException
{
    public override string Code => ErrorCodes.USER_EMPTY_LAST_NAME;

    public UserEmptyLastNameException(Guid id)
        : base($"User last name with id '{id}' can not be empty")
        => this.Id = id;

    public UserEmptyLastNameException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
