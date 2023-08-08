namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Entities;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal sealed class Ingredient
{
    public decimal Exchanger { get; set; } = default;
    public Guid FamilyId { get; set; } = Guid.Empty;
    public int Id { get; set; } = default;
    public Guid InternalId { get; set; } = Guid.Empty;
    public bool IsSystem { get; set; } = default;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string UnitSymbol { get; set; } = string.Empty;
}
