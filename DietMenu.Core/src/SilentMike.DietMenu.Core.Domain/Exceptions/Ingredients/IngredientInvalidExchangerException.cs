namespace SilentMike.DietMenu.Core.Domain.Exceptions.Ingredients;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Domain.Common;
using SilentMike.DietMenu.Core.Domain.Common.Constants;

[Serializable]
public sealed class IngredientInvalidExchangerException : DomainException
{
    public override string Code => ErrorCodes.INGREDIENT_INVALID_EXCHANGER;

    public IngredientInvalidExchangerException(Guid id, decimal exchanger)
        : base($"Ingredient exchanger with id 'id' can not be less than 0 ('{exchanger}')")
        => this.Id = id;

    private IngredientInvalidExchangerException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
