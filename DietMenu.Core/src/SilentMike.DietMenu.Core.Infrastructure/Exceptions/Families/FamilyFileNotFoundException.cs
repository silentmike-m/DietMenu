namespace SilentMike.DietMenu.Core.Infrastructure.Exceptions.Families;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Infrastructure.Common;
using SilentMike.DietMenu.Core.Infrastructure.Common.Constants;

[Serializable]
public sealed class FamilyFileNotFoundException : InfrastructureException
{
    public override string Code => ErrorCodes.FAMILY_FILE_NOT_FOUND;

    public FamilyFileNotFoundException(Guid familyId, string fileName)
        : base($"Family file with name '{fileName}' has not been found")
        => this.Id = familyId;

    private FamilyFileNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
