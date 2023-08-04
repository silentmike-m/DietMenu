namespace SilentMike.DietMenu.Core.Infrastructure.Exceptions.Families;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Infrastructure.Common;
using SilentMike.DietMenu.Core.Infrastructure.Common.Constants;

[Serializable]
public sealed class FamilyAlreadyExistsException : InfrastructureException
{
    public override string Code => ErrorCodes.FAMILY_ALREADY_EXISTS;

    public FamilyAlreadyExistsException(Guid id)
        : base($"Family with id '{id}' already exists")
        => this.Id = id;

    private FamilyAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
