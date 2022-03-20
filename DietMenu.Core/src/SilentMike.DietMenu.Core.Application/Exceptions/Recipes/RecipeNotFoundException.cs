namespace SilentMike.DietMenu.Core.Application.Exceptions.Recipes;

using System;
using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class RecipeNotFoundException : ApplicationException
{
    public override string Code => ErrorCodes.RECIPE_NOT_FOUND;

    public RecipeNotFoundException(Guid id)
        : base($"Recipe with id '{id}' has not been found")
    {
        this.Id = id;
    }

    private RecipeNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
