namespace SilentMike.DietMenu.Core.Application.Auth.Events;

public sealed record CreatedEmailConfirmationToken : INotification
{
    public string Email { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
}
