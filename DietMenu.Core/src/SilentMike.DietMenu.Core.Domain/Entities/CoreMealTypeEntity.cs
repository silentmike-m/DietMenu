namespace SilentMike.DietMenu.Core.Domain.Entities;

public sealed class CoreMealTypeEntity
{
    public CoreMealTypeEntity(Guid id) => this.Id = id;

    public Guid Id { get; private set; }
    public string InternalName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; } = default;
}
