namespace SilentMike.DietMenu.Core.Domain.Entities;

public sealed class RecipeIngredientEntity
{
    public RecipeIngredientEntity(Guid id) => this.Id = id;

    public Guid Id { get; private set; }
    public Guid IngredientId { get; set; } = Guid.Empty;
    public IngredientEntity Ingredient { get; set; } = null!;
    public Guid RecipeId { get; set; } = Guid.Empty;
    public RecipeEntity Recipe { get; set; } = null!;
    public int Quantity { get; set; } = default;
}
