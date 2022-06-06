namespace SilentMike.DietMenu.Core.Application.Exceptions.Ingredients;

using System.Runtime.Serialization;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class IngredientEmptyInternalNameException : ApplicationException
{
    public override string Code => ErrorCodes.INGREDIENT_EMPTY_INTERNAL_NAME;

    public IngredientEmptyInternalNameException()
        : base("Ingredient has empty internal name")
    {
    }

    private IngredientEmptyInternalNameException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
