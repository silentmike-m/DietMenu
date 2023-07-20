namespace SilentMike.DietMenu.Auth.Application.Users.Commands;

using SilentMike.DietMenu.Auth.Application.Users.ValueModels;

public sealed record CreateUser : IRequest
{
    [JsonPropertyName("user")] public UserToCreate User { get; init; } = new();
}
