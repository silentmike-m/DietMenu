namespace SilentMike.DietMenu.Auth.Application.Users.ViewModels;

public sealed record User
{
    [JsonPropertyName("id")] public string Id { get; init; } = string.Empty;
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
    [JsonPropertyName("family_id")] public Guid FamilyId { get; init; } = Guid.Empty;
    [JsonPropertyName("first_name")] public string FirstName { get; init; } = string.Empty;
    [JsonPropertyName("activated")] public bool IsActivated { get; init; } = default;
    [JsonPropertyName("last_name")] public string LastName { get; init; } = string.Empty;
    [JsonPropertyName("phone_number")] public string PhoneNumber { get; init; } = string.Empty;

}
