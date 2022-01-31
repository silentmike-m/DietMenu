namespace SilentMike.DietMenu.Core.Domain.Entities;

public sealed class FamilyEntity
{
    public FamilyEntity() { }

    public FamilyEntity(Guid id) => this.FamilyId = id;

    public Guid FamilyId { get; private set; }
    public int Id { get; private set; }
    public string Name { get; set; } = string.Empty;
    public List<MealTypeEntity> MealTypes { get; set; } = new();
}
