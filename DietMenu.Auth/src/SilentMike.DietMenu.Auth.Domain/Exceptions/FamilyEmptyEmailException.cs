namespace SilentMike.DietMenu.Auth.Domain.Exceptions;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Domain.Common;
using SilentMike.DietMenu.Auth.Domain.Common.Constants;

[Serializable]
public sealed class FamilyEmptyEmailException : DomainException
{
    public override string Code => ErrorCodes.FAMILY_EMPTY_EMAIL;

    public FamilyEmptyEmailException(Guid id)
        : base($"Family email with id '{id}' can not be empty")
        => this.Id = id;

    private FamilyEmptyEmailException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
