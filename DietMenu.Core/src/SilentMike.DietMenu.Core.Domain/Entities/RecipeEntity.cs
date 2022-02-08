namespace SilentMike.DietMenu.Core.Domain.Entities;

public sealed class RecipeEntity
{
    public RecipeEntity(Guid id) => this.Id = id;

    public Guid Id { get; private set; }
    public decimal Carbohydrates { get; set; } = default;
    public string Description { get; set; } = string.Empty;
    public decimal Energy { get; set; } = default;
    public Guid FamilyId { get; set; } = Guid.Empty;
    public FamilyEntity FamilyEntity { get; set; } = null!;
    public decimal Fat { get; set; } = default;
    public List<RecipeIngredientEntity> Ingredients { get; set; } = null!;
    public MealTypeEntity MealType { get; set; } = null!;
    public Guid MealTypeId { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public int Protein { get; set; } = default;
    public Guid UserId { get; set; } = Guid.Empty;
}
