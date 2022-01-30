namespace SilentMike.DietMenu.Auth.Application.Users.Events;

public sealed record CreatedUser : INotification
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
}
