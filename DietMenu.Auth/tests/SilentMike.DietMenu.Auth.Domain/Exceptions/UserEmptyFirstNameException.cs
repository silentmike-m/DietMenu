namespace SilentMike.DietMenu.Auth.Domain.Exceptions;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Domain.Common;
using SilentMike.DietMenu.Auth.Domain.Common.Constants;

[Serializable]
public sealed class UserEmptyFirstNameException : DomainException
{
    public override string Code => ErrorCodes.USER_EMPTY_FIRST_NAME;

    public UserEmptyFirstNameException(Guid id)
        : base($"User first name with id '{id}' can not be empty")
        => this.Id = id;

    public UserEmptyFirstNameException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
