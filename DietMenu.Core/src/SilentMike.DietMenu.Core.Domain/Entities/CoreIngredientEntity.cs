namespace SilentMike.DietMenu.Core.Domain.Entities;

public sealed class CoreIngredientEntity
{
    public CoreIngredientEntity(Guid id) => this.Id = id;

    public Guid Id { get; private set; }
    public decimal Exchanger { get; set; } = 1;
    public string InternalName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string Name { get; set; } = string.Empty;
    public Guid TypeId { get; set; } = Guid.Empty;
    public CoreIngredientTypeEntity Type { get; set; } = null!;
    public string UnitSymbol { get; set; } = string.Empty;
}
