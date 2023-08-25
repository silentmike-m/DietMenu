namespace SilentMike.DietMenu.Core.Infrastructure.Exceptions.Ingredients;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Infrastructure.Common;
using SilentMike.DietMenu.Core.Infrastructure.Common.Constants;

[Serializable]
public sealed class IngredientToImportInvalidTypeException : InfrastructureException
{
    public override string Code => ErrorCodes.FAMILY_FILE_NOT_FOUND;

    public IngredientToImportInvalidTypeException(Guid ingredientId, string ingredientType, string targetIngredientType)
        : base($"Ingredient with id '{ingredientId}' and type '{ingredientType}' is inconsistent with target '{targetIngredientType}'")
        => this.Id = ingredientId;

    private IngredientToImportInvalidTypeException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
