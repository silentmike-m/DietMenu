namespace SilentMike.DietMenu.Core.Domain.Entities;

public sealed class FamilyEntity
{
    public FamilyEntity() { }

    public FamilyEntity(Guid id) => this.Id = id;

    public Guid Id { get; private set; }
    public List<MealTypeEntity> MealTypes { get; set; } = null!;
    public List<IngredientTypeEntity> IngredientTypes { get; set; } = null!;
    public List<IngredientEntity> Ingredients { get; set; } = null!;
}
