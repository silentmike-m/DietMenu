namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Entities;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal sealed class IngredientEntity
{
    public double Exchanger { get; set; } = default;
    public Guid FamilyId { get; set; } = Guid.NewGuid();
    public int Id { get; set; } = default;
    public Guid IngredientId { get; set; } = Guid.NewGuid();
    public bool IsActive { get; set; } = default;
    public bool IsSystem { get; set; } = default;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string UnitSymbol { get; set; } = string.Empty;
}
