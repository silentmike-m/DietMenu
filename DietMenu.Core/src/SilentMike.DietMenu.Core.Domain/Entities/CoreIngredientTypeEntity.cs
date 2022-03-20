namespace SilentMike.DietMenu.Core.Domain.Entities;

public sealed class CoreIngredientTypeEntity
{
    public CoreIngredientTypeEntity(Guid id) => this.Id = id;
    public Guid Id { get; private set; }
    public string InternalName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
