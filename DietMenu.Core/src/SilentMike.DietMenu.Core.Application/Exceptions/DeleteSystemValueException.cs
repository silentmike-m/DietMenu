namespace SilentMike.DietMenu.Core.Application.Exceptions;

using System;
using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class DeleteSystemValueException : ApplicationException
{
    public override string Code => ErrorCodes.DELETE_SYSTEM_VALUE;

    public DeleteSystemValueException(Guid id, string name)
        : base($"Cannot delete system value of \"{name}\" with id {id}")
    {
        this.Id = id;
    }

    private DeleteSystemValueException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}