namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models;

internal sealed class Family
{
    public int Id { get; set; } = default;
    public Guid InternalId { get; set; } = Guid.Empty;
}
