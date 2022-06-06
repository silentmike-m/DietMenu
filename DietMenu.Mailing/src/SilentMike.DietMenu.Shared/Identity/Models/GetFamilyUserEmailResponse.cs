namespace SilentMike.DietMenu.Shared.Identity.Models;

public sealed record GetFamilyUserEmailResponse
{
    public string Email { get; init; } = string.Empty;
}
