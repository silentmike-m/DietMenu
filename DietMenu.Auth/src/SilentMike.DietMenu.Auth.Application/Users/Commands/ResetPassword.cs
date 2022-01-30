namespace SilentMike.DietMenu.Auth.Application.Users.Commands;

public sealed record ResetPassword : IRequest
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
    [JsonPropertyName("password")] public string Password { get; init; } = string.Empty;
    [JsonPropertyName("token")] public string Token { get; init; } = string.Empty;
}
