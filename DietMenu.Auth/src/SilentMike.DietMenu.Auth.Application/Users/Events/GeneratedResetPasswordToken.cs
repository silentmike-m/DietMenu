namespace SilentMike.DietMenu.Auth.Application.Users.Events;

public sealed record GeneratedResetPasswordToken : INotification
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
    [JsonPropertyName("token")] public string Token { get; init; } = string.Empty;
}
