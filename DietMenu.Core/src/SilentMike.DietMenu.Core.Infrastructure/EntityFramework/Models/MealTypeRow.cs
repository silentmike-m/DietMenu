namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models;

public sealed class MealTypeRow
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid FamilyId { get; set; } = Guid.Empty;
    public bool IsActive { get; set; } = default;
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; } = default;
}
