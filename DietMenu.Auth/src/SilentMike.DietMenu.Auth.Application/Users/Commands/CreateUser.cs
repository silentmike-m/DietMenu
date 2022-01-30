namespace SilentMike.DietMenu.Auth.Application.Users.Commands;

public sealed class CreateUser : IRequest
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
    [JsonPropertyName("family")] public string Family { get; init; } = string.Empty;
    [JsonPropertyName("first_name")] public string FirstName { get; init; } = string.Empty;
    [JsonPropertyName("last_name")] public string LastName { get; init; } = string.Empty;
    [JsonPropertyName("password")] public string Password { get; init; } = string.Empty;
    [JsonPropertyName("register_code")] public string RegisterCode { get; init; } = string.Empty;
}
