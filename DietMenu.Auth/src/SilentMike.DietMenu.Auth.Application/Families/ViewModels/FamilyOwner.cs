namespace SilentMike.DietMenu.Auth.Application.Families.ViewModels;

public sealed record FamilyOwner
{
    public string Email { get; init; } = string.Empty;
    public Guid UserId { get; init; } = Guid.Empty;
}
