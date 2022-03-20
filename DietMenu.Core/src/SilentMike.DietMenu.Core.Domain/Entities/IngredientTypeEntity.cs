namespace SilentMike.DietMenu.Core.Domain.Entities;

public sealed class IngredientTypeEntity
{
    public IngredientTypeEntity(Guid id) => this.Id = id;

    public Guid Id { get; private set; }

    public Guid FamilyId { get; set; } = Guid.Empty;
    public FamilyEntity FamilyEntity { get; set; } = null!;
    public string InternalName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string Name { get; set; } = string.Empty;
}
