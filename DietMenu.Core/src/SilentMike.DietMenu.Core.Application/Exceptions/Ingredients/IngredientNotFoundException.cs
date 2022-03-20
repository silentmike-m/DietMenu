namespace SilentMike.DietMenu.Core.Application.Exceptions.Ingredients;

using System;
using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class IngredientNotFoundException : ApplicationException
{
    public override string Code => ErrorCodes.INGREDIENT_NOT_FOUND;

    public IngredientNotFoundException(Guid id)
        : base($"Ingredient with id '{id}' has not been found")
    {
        this.Id = id;
    }

    private IngredientNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
