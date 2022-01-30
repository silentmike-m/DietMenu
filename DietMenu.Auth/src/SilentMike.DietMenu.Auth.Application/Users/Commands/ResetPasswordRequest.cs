namespace SilentMike.DietMenu.Auth.Application.Users.Commands;

public sealed record ResetPasswordRequest : IRequest
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
}
