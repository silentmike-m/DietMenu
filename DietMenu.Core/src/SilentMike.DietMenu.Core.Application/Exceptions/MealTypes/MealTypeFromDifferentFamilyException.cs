namespace SilentMike.DietMenu.Core.Application.Exceptions.MealTypes;

using System;
using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class MealTypeFromDifferentFamilyException : ApplicationException
{
    public override string Code => ErrorCodes.USER_NOT_FOUND;

    public MealTypeFromDifferentFamilyException(Guid mealTypeId)
        : base($"Meal type with id {mealTypeId} is from different family")
    {
        this.Id = Guid.Empty;
    }

    private MealTypeFromDifferentFamilyException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
