namespace SilentMike.DietMenu.Auth.Application.Auth.ViewModels;

public sealed record UserClaims
{
    public Dictionary<string, string> Claims { get; init; } = new();
    public Guid FamilyId { get; set; } = Guid.Empty;
    public Guid UserId { get; init; } = Guid.Empty;
}
