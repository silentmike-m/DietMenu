namespace SilentMike.DietMenu.Core.Application.Exceptions.IngredientTypes;

using System;
using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class IngredientTypeNotFoundException : ApplicationException
{
    public override string Code => ErrorCodes.INGREDIENT_TYPE_NOT_FOUND;

    public IngredientTypeNotFoundException(Guid id)
        : base($"Ingredient type with id {id} has not been found")
    {
        this.Id = id;
    }

    private IngredientTypeNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
