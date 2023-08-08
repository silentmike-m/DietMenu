namespace SilentMike.DietMenu.Core.Application.Common.Models;

public sealed record AuthData : IAuthData
{
    public Guid FamilyId { get; set; } = default;
    public Guid UserId { get; set; } = Guid.Empty;
}
