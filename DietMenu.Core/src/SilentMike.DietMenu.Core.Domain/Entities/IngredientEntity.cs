namespace SilentMike.DietMenu.Core.Domain.Entities;

public sealed class IngredientEntity
{
    public IngredientEntity(Guid id) => this.Id = id;

    public Guid Id { get; private set; }
    public decimal Exchanger { get; set; } = 1;
    public Guid FamilyId { get; set; } = Guid.Empty;
    public FamilyEntity FamilyEntity { get; set; } = null!;
    public string InternalName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsSystem { get; set; } = default;
    public string Name { get; set; } = string.Empty;
    public Guid TypeId { get; set; } = Guid.Empty;
    public IngredientTypeEntity Type { get; set; } = null!;
    public string UnitSymbol { get; set; } = string.Empty;
}
