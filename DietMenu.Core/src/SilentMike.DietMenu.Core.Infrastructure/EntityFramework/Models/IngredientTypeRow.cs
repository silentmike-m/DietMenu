namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models;

internal sealed class IngredientTypeRow
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid FamilyId { get; set; } = Guid.Empty;
    public bool IsActive { get; set; } = default;
    public string Name { get; init; } = string.Empty;
}
