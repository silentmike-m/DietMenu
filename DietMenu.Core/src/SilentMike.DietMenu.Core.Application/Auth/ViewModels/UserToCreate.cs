namespace SilentMike.DietMenu.Core.Application.Auth.ViewModels;

using System.Text.Json.Serialization;

public sealed record UserToCreate
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
    [JsonPropertyName("family_name")] public string FamilyName { get; init; } = string.Empty;
    [JsonPropertyName("first_name")] public string FirstName { get; init; } = string.Empty;
    [JsonPropertyName("last_name")] public string LastName { get; init; } = string.Empty;
    [JsonPropertyName("password")] public string Password { get; init; } = string.Empty;
    [JsonPropertyName("user_name")] public string UserName { get; init; } = string.Empty;
}
