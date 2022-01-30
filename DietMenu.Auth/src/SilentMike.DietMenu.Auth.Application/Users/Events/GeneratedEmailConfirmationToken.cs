namespace SilentMike.DietMenu.Auth.Application.Users.Events;

public sealed record GeneratedEmailConfirmationToken : INotification
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
    [JsonPropertyName("token")] public string Token { get; init; } = string.Empty;
    [JsonPropertyName("user_id")] public Guid UserId { get; init; } = Guid.Empty;
}

