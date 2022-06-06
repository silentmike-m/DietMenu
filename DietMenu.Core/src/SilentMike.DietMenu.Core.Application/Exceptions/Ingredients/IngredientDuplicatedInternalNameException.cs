namespace SilentMike.DietMenu.Core.Application.Exceptions.Ingredients;

using System.Runtime.Serialization;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class IngredientDuplicatedInternalNameException : ApplicationException
{
    public override string Code => ErrorCodes.INGREDIENT_DUPLICATED_INTERNAL_NAME;

    public IngredientDuplicatedInternalNameException(string internalName)
        : base($"Duplicated ingredient with internal name '{internalName}'")
    {
    }

    private IngredientDuplicatedInternalNameException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
