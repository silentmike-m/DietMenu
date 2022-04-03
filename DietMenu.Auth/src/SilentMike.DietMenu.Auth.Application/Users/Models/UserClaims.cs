namespace SilentMike.DietMenu.Auth.Application.Users.Models;

using System.Security.Claims;

public sealed record UserClaims
{
    public IReadOnlyList<Claim> Claims { get; init; } = new List<Claim>().AsReadOnly();
    public string UserId { get; init; } = string.Empty;
}
