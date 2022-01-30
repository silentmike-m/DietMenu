namespace SilentMike.DietMenu.Auth.Application.Users.Commands;

public sealed record ConfirmUser : IRequest
{
    [JsonPropertyName("id"), Required] public Guid Id { get; init; } = Guid.Empty;
    [JsonPropertyName("token"), Required] public string Token { get; init; } = string.Empty;
}
