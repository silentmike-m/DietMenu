namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Entities;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal sealed class FamilyEntity
{
    public Guid FamilyId { get; set; } = Guid.NewGuid();
    public int Id { get; set; } = default;
    public Dictionary<string, string> IngredientsVersion { get; set; } = new();
}
