namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class Family
{
    public string Email { get; set; } = string.Empty;
    public Guid Id { get; set; } = Guid.Empty;
    public int Key { get; set; } = default;
    public string Name { get; set; } = string.Empty;
}
