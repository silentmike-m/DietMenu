namespace SilentMike.DietMenu.Auth.Application.Users.Commands;

public sealed record UpdateUserLastLoginDate : IRequest
{
    public Guid UserId { get; init; } = Guid.Empty;
}
