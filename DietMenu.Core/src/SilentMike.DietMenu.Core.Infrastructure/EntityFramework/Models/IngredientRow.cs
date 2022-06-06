namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models;

internal sealed class IngredientRow
{
    public Guid Id { get; set; } = Guid.Empty;
    public decimal Exchanger { get; set; } = default;
    public Guid FamilyId { get; set; } = Guid.Empty;
    public bool IsActive { get; set; } = default;
    public string Name { get; set; } = string.Empty;
    public Guid TypeId { get; set; } = Guid.Empty;
    public string TypeName { get; set; } = string.Empty;
    public string UnitSymbol { get; set; } = string.Empty;
}
