namespace SilentMike.DietMenu.Core.Domain.Exceptions.Ingredients;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Domain.Common;
using SilentMike.DietMenu.Core.Domain.Common.Constants;

[Serializable]
public sealed class IngredientInvalidTypeException : DomainException
{
    public override string Code => ErrorCodes.INGREDIENT_INVALID_TYPE;

    public IngredientInvalidTypeException(Guid id, string type)
        : base($"Ingredient type with id 'id' is invalid ('{type}')")
        => this.Id = id;

    private IngredientInvalidTypeException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
