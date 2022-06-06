namespace SilentMike.DietMenu.Core.Application.Exceptions.IngredientTypes;

using System.Runtime.Serialization;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class IngredientTypeNotFoundException : ApplicationException
{
    public override string Code => ErrorCodes.INGREDIENT_TYPE_NOT_FOUND;

    public IngredientTypeNotFoundException(Guid id)
        : base($"Ingredient type with id '{id}' has not been found")
    {
        this.Id = id;
    }

    public IngredientTypeNotFoundException(string internalName)
        : base($"Ingredient type with internal name '{internalName}' has not been found")
    {
    }

    private IngredientTypeNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
