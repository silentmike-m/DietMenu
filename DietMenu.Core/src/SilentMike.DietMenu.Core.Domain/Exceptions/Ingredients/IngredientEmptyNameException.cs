namespace SilentMike.DietMenu.Core.Domain.Exceptions.Ingredients;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Domain.Common;
using SilentMike.DietMenu.Core.Domain.Common.Constants;

[Serializable]
public sealed class IngredientEmptyNameException : DomainException
{
    public override string Code => ErrorCodes.INGREDIENT_EMPTY_NAME;

    public IngredientEmptyNameException(Guid id)
        : base($"Ingredient name with id '{id}' can not be empty")
        => this.Id = id;

    private IngredientEmptyNameException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
