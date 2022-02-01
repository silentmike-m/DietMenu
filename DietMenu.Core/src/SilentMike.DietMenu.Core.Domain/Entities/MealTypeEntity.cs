namespace SilentMike.DietMenu.Core.Domain.Entities;

public sealed class MealTypeEntity
{
    public MealTypeEntity() { }

    public MealTypeEntity(Guid id) => this.Id = id;

    public Guid Id { get; private set; }
    public Guid FamilyId { get; set; } = Guid.Empty;
    public FamilyEntity? FamilyEntity { get; set; } = default;
    public string InternalName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; } = default;
}
