namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Entities;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal sealed class FamilyEntity
{
    public int Id { get; set; } = default;
    public string IngredientsVersion { get; set; } = string.Empty;
    public Guid InternalId { get; set; } = Guid.Empty;
}
