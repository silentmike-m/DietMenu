namespace SilentMike.DietMenu.Core.Application.Exceptions.Families;

using System;
using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class FamilyAlreadyExistsException : ApplicationException
{
    public override string Code => ErrorCodes.FAMILY_ALREADY_EXISTS;

    public FamilyAlreadyExistsException(string name)
        : base($"Family with name {name} already exists")
    {
        this.Id = Guid.Empty;
    }

    private FamilyAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
