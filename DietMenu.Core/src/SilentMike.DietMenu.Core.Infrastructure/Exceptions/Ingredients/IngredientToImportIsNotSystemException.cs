namespace SilentMike.DietMenu.Core.Infrastructure.Exceptions.Ingredients;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Infrastructure.Common;
using SilentMike.DietMenu.Core.Infrastructure.Common.Constants;

[Serializable]
public sealed class IngredientToImportIsNotSystemException : InfrastructureException
{
    public override string Code => ErrorCodes.FAMILY_FILE_NOT_FOUND;

    public IngredientToImportIsNotSystemException(Guid ingredientId)
        : base($"Ingredient with id '{ingredientId}' is not system ingredient")
        => this.Id = ingredientId;

    private IngredientToImportIsNotSystemException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
