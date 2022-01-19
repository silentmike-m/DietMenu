namespace SilentMike.DietMenu.Mailing.Application.Users.Commands;

using System.Text.Json.Serialization;

public sealed record SendCreatedUserEmail : IRequest
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
    [JsonPropertyName("family_name")] public string FamilyName { get; init; } = string.Empty;
    [JsonPropertyName("login_url")] public string LoginUrl { get; init; } = string.Empty;
    [JsonPropertyName("user_name")] public string UserName { get; init; } = string.Empty;
}
