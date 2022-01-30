namespace SilentMike.DietMenu.Auth.Application.Users.Commands;

public sealed record SendUserConfirmation : IRequest
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
}
