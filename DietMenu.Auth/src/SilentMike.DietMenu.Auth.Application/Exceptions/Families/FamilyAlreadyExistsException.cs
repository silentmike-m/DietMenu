namespace SilentMike.DietMenu.Auth.Application.Exceptions.Families;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Application.Common;

[Serializable]
public sealed class FamilyAlreadyExistsException : ApplicationException
{
    public override string Code => ErrorCodes.FAMILY_ALREADY_EXISTS;

    public FamilyAlreadyExistsException(Guid id)
        : base($"Family with id '{id}' already exists")
        => this.Id = id;

    public FamilyAlreadyExistsException(string name)
        : base($"Family with name '{name}' already exists")
    {
    }

    private FamilyAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
