namespace SilentMike.DietMenu.Shared.Identity.Models;

public sealed record GetSystemUserEmailResponse
{
    public string Email { get; init; } = string.Empty;
}
