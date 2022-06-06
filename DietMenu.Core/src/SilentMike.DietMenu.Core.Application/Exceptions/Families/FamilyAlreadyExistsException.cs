namespace SilentMike.DietMenu.Core.Application.Exceptions.Families;

using System.Runtime.Serialization;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class FamilyAlreadyExistsException : ApplicationException
{
    public override string Code => ErrorCodes.FAMILY_ALREADY_EXISTS;

    public FamilyAlreadyExistsException(Guid id)
        : base($"Family with id '{id}' already exists")
    {
        this.Id = id;
    }

    private FamilyAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
