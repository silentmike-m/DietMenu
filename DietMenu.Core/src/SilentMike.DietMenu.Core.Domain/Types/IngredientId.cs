namespace SilentMike.DietMenu.Core.Domain.Types;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public sealed class IngredientId : StronglyTypedValue<Guid>
{
    public IngredientId(Guid value)
        : base(value)
    {
    }
}
