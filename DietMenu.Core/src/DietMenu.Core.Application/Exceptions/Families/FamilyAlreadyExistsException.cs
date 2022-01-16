namespace DietMenu.Core.Application.Exceptions.Families;

using System;
using System.Runtime.Serialization;
using DietMenu.Core.Application.Common.Constants;

[Serializable]
public sealed class FamilyAlreadyExistsException : Core.Application.Common.ApplicationException
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
