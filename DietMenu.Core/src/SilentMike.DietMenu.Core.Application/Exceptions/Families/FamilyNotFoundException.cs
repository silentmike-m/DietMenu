namespace SilentMike.DietMenu.Core.Application.Exceptions.Families;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common;

[Serializable]
public sealed class FamilyNotFoundException : ApplicationException
{
    public override string Code => ErrorCodes.FAMILY_NOT_FOUND;

    public FamilyNotFoundException(Guid id)
        : base($"Family with id '{id}' has not been found")
    {
        this.Id = id;
    }

    private FamilyNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
