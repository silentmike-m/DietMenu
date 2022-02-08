namespace SilentMike.DietMenu.Core.Application.Exceptions.MealTypes;

using System;
using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class MealTypeNotFoundException : ApplicationException
{
    public override string Code => ErrorCodes.MEAL_TYPE_NOT_FOUND;

    public MealTypeNotFoundException(Guid id)
        : base($"Meal type with id {id} has not been found")
    {
        this.Id = id;
    }

    private MealTypeNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
