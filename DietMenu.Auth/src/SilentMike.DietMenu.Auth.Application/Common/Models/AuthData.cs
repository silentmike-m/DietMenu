namespace SilentMike.DietMenu.Auth.Application.Common.Models;

public sealed record AuthData : IAuthData
{
    public Guid FamilyId { get; set; } = Guid.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
}
