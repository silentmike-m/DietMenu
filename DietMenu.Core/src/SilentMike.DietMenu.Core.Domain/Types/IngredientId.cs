namespace SilentMike.DietMenu.Core.Domain.Types;

public sealed class IngredientId : StronglyTypedValue<Guid>
{
    public IngredientId(Guid value)
        : base(value)
    {
    }
}
