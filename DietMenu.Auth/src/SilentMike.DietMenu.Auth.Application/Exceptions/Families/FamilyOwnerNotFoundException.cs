namespace SilentMike.DietMenu.Auth.Application.Exceptions.Families;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Application.Common;

[Serializable]
public sealed class FamilyOwnerNotFoundException : ApplicationException
{
    public override string Code => ErrorCodes.FAMILY_OWNER_NOT_FOUND;

    public FamilyOwnerNotFoundException(Guid id)
        : base($"Family with id '{id}'owner has not been found")
        => this.Id = id;

    private FamilyOwnerNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
