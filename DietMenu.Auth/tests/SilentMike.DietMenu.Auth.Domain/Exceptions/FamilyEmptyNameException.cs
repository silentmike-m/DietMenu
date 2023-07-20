namespace SilentMike.DietMenu.Auth.Domain.Exceptions;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Domain.Common;
using SilentMike.DietMenu.Auth.Domain.Common.Constants;

[Serializable]
public sealed class FamilyEmptyNameException : DomainException
{
    public override string Code => ErrorCodes.FAMILY_EMPTY_NAME;

    public FamilyEmptyNameException(Guid id)
        : base($"Family name with id '{id}' can not be empty")
        => this.Id = id;

    public FamilyEmptyNameException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
